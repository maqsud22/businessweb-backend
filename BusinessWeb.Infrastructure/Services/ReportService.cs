using BusinessWeb.Application.DTOs.Reports;
using BusinessWeb.Application.Exceptions;
using BusinessWeb.Application.Interfaces.Reports;
using BusinessWeb.Domain.Enums;
using BusinessWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _db;

    public ReportService(AppDbContext db) => _db = db;

    public async Task<SalesSummaryDto> GetDailyAsync(DateTime? date, CancellationToken ct = default)
    {
        var day = (date ?? DateTime.UtcNow).Date;
        var to = day.AddDays(1);
        return await BuildSalesSummaryAsync(day, to, ct);
    }

    public async Task<SalesSummaryDto> GetMonthlyAsync(DateTime? date, CancellationToken ct = default)
    {
        var refDate = date ?? DateTime.UtcNow;
        var start = new DateTime(refDate.Year, refDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = start.AddMonths(1);
        return await BuildSalesSummaryAsync(start, end, ct);
    }

    public async Task<ProductReportDto> GetProductAsync(Guid productId, CancellationToken ct = default)
    {
        var product = await _db.Products.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == productId, ct);

        if (product is null)
            throw new AppException("Product topilmadi.", 404, "not_found");

        var incoming = await _db.StockIns.AsNoTracking()
            .Where(x => x.ProductId == productId)
            .SumAsync(x => x.Quantity * x.ProductPackage.Multiplier, ct);

        var sold = await _db.SaleLines.AsNoTracking()
            .Where(x => x.ProductId == productId)
            .SumAsync(x => x.Quantity * x.ProductPackage.Multiplier, ct);

        return new ProductReportDto
        {
            ProductId = product.Id,
            ProductName = product.Name,
            IncomingBaseQty = incoming,
            SoldBaseQty = sold,
            InStockBaseQty = incoming - sold
        };
    }

    public async Task<IReadOnlyList<StockReportItemDto>> GetStockAsync(CancellationToken ct = default)
    {
        var products = await _db.Products.AsNoTracking()
            .Select(x => new { x.Id, x.Name })
            .ToListAsync(ct);

        var incoming = await _db.StockIns.AsNoTracking()
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                BaseQty = g.Sum(x => x.Quantity * x.ProductPackage.Multiplier)
            })
            .ToListAsync(ct);

        var sold = await _db.SaleLines.AsNoTracking()
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                BaseQty = g.Sum(x => x.Quantity * x.ProductPackage.Multiplier)
            })
            .ToListAsync(ct);

        var incomingMap = incoming.ToDictionary(x => x.ProductId, x => x.BaseQty);
        var soldMap = sold.ToDictionary(x => x.ProductId, x => x.BaseQty);

        return products.Select(p =>
        {
            var inBase = incomingMap.TryGetValue(p.Id, out var inQty) ? inQty : 0m;
            var soldBase = soldMap.TryGetValue(p.Id, out var soldQty) ? soldQty : 0m;

            return new StockReportItemDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                IncomingBaseQty = inBase,
                SoldBaseQty = soldBase,
                InStockBaseQty = inBase - soldBase
            };
        }).ToList();
    }

    private async Task<SalesSummaryDto> BuildSalesSummaryAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        var sales = await _db.Sales.AsNoTracking()
            .Where(x => x.CreatedAt >= from && x.CreatedAt < to)
            .Select(x => new { x.TotalAmount, x.PaymentType })
            .ToListAsync(ct);

        var debtPayments = await _db.DebtPayments.AsNoTracking()
            .Where(x => x.Date >= from && x.Date < to)
            .SumAsync(x => x.Amount, ct);

        return new SalesSummaryDto
        {
            From = from,
            To = to,
            TotalSales = sales.Sum(x => x.TotalAmount),
            CashSales = sales.Where(x => x.PaymentType == PaymentType.Cash).Sum(x => x.TotalAmount),
            DebtSales = sales.Where(x => x.PaymentType == PaymentType.Debt).Sum(x => x.TotalAmount),
            PartialSales = sales.Where(x => x.PaymentType == PaymentType.Partial).Sum(x => x.TotalAmount),
            DebtPayments = debtPayments,
            SaleCount = sales.Count
        };
    }
}
