using AutoMapper;
using BusinessWeb.Application.DTOs.Sales;
using BusinessWeb.Application.Exceptions;
using BusinessWeb.Application.Interfaces;
using BusinessWeb.Application.Interfaces.Sales;
using BusinessWeb.Domain.Entities;
using BusinessWeb.Domain.Enums;
using BusinessWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
 codex/finalize-production-ready-backend-for-businessweb-3s06y9

namespace BusinessWeb.Infrastructure.Services;

public class SaleService : ISaleService
{
    private readonly AppDbContext _db;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ILogger<SaleService> _logger;

    public SaleService(AppDbContext db, IUnitOfWork uow, IMapper mapper, ILogger<SaleService> logger)
    {
        _db = db;
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<SaleListItemDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Sales.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<SaleListItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task<SaleDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var sale = await _db.Sales.AsNoTracking()
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return sale is null ? null : _mapper.Map<SaleDetailDto>(sale);
    }

    public async Task<SaleResultDto> CreateAsync(CreateSaleDto dto, CancellationToken ct = default)
    {
        // 1) Store mavjudmi?
        var storeExists = await _uow.Repo<Store>().Query()
            .AsNoTracking()
            .AnyAsync(s => s.Id == dto.StoreId, ct);

        if (!storeExists)
            throw new AppException("Store topilmadi.", 404, "not_found");

        // 2) Packages tekshiruv
        var packageIds = dto.Lines.Select(x => x.ProductPackageId).Distinct().ToList();
        var packages = await _uow.Repo<ProductPackage>().Query()
            .AsNoTracking()
            .Where(p => packageIds.Contains(p.Id))
            .ToListAsync(ct);

        if (packages.Count != packageIds.Count)
            throw new AppException("Ba'zi ProductPackage topilmadi.", 404, "not_found");

        // Product <-> Package mosligini tekshirish
        foreach (var line in dto.Lines)
        {
            var pkg = packages.First(p => p.Id == line.ProductPackageId);
            if (pkg.ProductId != line.ProductId)
                throw new AppException("ProductPackage tanlangan Product ga tegishli emas.", 400, "validation_error");
        }

        // 3) Stock check (base birlik)
        var requiredBaseByProduct = dto.Lines
            .GroupBy(l => l.ProductId)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(l =>
                {
                    var m = packages.First(p => p.Id == l.ProductPackageId).Multiplier;
                    return l.Quantity * m;
                })
            );

        var productIds = requiredBaseByProduct.Keys.ToList();

        // Incoming base qty
        var incoming = await _uow.Repo<StockIn>().Query()
            .AsNoTracking()
            .Where(x => productIds.Contains(x.ProductId))
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                BaseQty = g.Sum(x => x.Quantity * x.ProductPackage.Multiplier)
            })
            .ToListAsync(ct);

        // Sold base qty
        var sold = await _uow.Repo<SaleLine>().Query()
            .AsNoTracking()
            .Where(x => productIds.Contains(x.ProductId))
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                BaseQty = g.Sum(x => x.Quantity * x.ProductPackage.Multiplier)
            })
            .ToListAsync(ct);

        var incomingMap = incoming.ToDictionary(x => x.ProductId, x => x.BaseQty);
        var soldMap = sold.ToDictionary(x => x.ProductId, x => x.BaseQty);

        foreach (var kv in requiredBaseByProduct)
        {
            var pid = kv.Key;
            var required = kv.Value;

            var inBase = incomingMap.TryGetValue(pid, out var v1) ? v1 : 0m;
            var soldBase = soldMap.TryGetValue(pid, out var v2) ? v2 : 0m;

            var available = inBase - soldBase;

            if (available < required)
                throw new AppException($"Stock yetarli emas. ProductId={pid}. Available={available}, Required={required}", 409, "conflict");
        }

        // 4) Transaction
        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            StoreId = dto.StoreId,
            PaymentType = dto.PaymentType,
            CreatedAt = DateTime.UtcNow
        };

        var lines = dto.Lines.Select(l =>
        {
            var entity = _mapper.Map<SaleLine>(l);
            entity.Id = Guid.NewGuid();
            entity.SaleId = sale.Id;
            entity.CreatedAt = DateTime.UtcNow;
            entity.LineTotal = l.Quantity * l.UnitPrice; // 1 package narxiga ko'ra
            return entity;
        }).ToList();

        var total = lines.Sum(x => x.LineTotal);

        // Payment rules
        if (dto.PaymentType == PaymentType.Cash)
        {
            if (dto.PaidAmount != total)
                throw new AppException("Cash bo'lsa PaidAmount totalga teng bo'lishi kerak.", 400, "validation_error");
        }
        else if (dto.PaymentType == PaymentType.Debt)
        {
            if (dto.PaidAmount != 0)
                throw new AppException("Debt bo'lsa PaidAmount 0 bo'lishi kerak.", 400, "validation_error");
        }
        else if (dto.PaymentType == PaymentType.Partial)
        {
            if (dto.PaidAmount <= 0 || dto.PaidAmount >= total)
                throw new AppException("Partial bo'lsa 0 < PaidAmount < Total bo'lishi shart.", 400, "validation_error");
        }

        // Sale’da faqat total saqlanadi (sizning modelingiz shunaqa)
        sale.TotalAmount = total;

        await _uow.Repo<Sale>().AddAsync(sale, ct);
        foreach (var line in lines)
            await _uow.Repo<SaleLine>().AddAsync(line, ct);

        Debt? debt = null;

        if (dto.PaymentType is PaymentType.Debt or PaymentType.Partial)
        {
            var remaining = total - dto.PaidAmount;

            debt = new Debt
            {
                Id = Guid.NewGuid(),
                SaleId = sale.Id,
                Total = total,
                Paid = dto.PaidAmount,
                IsClosed = remaining <= 0,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Repo<Debt>().AddAsync(debt, ct);

            // Partial bo'lsa birinchi payment record (audit)
            if (dto.PaymentType == PaymentType.Partial && dto.PaidAmount > 0)
            {
                var payment = new DebtPayment
                {
                    Id = Guid.NewGuid(),
                    DebtId = debt.Id,
                    Amount = dto.PaidAmount,
                    Date = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                await _uow.Repo<DebtPayment>().AddAsync(payment, ct);
            }
        }

        await _uow.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        _logger.LogInformation("Sale created. SaleId={SaleId}, StoreId={StoreId}, Total={Total}", sale.Id, sale.StoreId, sale.TotalAmount);

        return new SaleResultDto
        {
            SaleId = sale.Id,
            StoreId = sale.StoreId,
            PaymentType = sale.PaymentType,
            TotalAmount = sale.TotalAmount,
            PaidAmount = dto.PaidAmount,
            RemainingAmount = total - dto.PaidAmount,
            DebtId = debt?.Id,
            DebtIsClosed = debt?.IsClosed
        };
    }
}

using Microsoft.Extensions.Logging;
 main
