using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessWeb.Application.DTOs.StockIns;
using BusinessWeb.Application.Exceptions;
using BusinessWeb.Application.Interfaces;
using BusinessWeb.Application.Interfaces.StockIns;
using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Infrastructure.Services;

public class StockInService : IStockInService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public StockInService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public Task<List<StockInListItemDto>> GetAllAsync(CancellationToken ct = default)
        => _uow.Repo<StockIn>().Query()
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .ProjectTo<StockInListItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

    public Task<StockInDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _uow.Repo<StockIn>().Query()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<StockInDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);

    public async Task<StockInDetailDto> CreateAsync(StockInCreateDto dto, CancellationToken ct = default)
    {
        await EnsureProductPackageAsync(dto.ProductId, dto.ProductPackageId, ct);

        var entity = _mapper.Map<StockIn>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        await _uow.Repo<StockIn>().AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return await GetByIdAsync(entity.Id, ct) ?? _mapper.Map<StockInDetailDto>(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, StockInUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Repo<StockIn>().GetByIdAsync(id, ct);
        if (entity is null) return false;

        await EnsureProductPackageAsync(dto.ProductId, dto.ProductPackageId, ct);

        _mapper.Map(dto, entity);
        _uow.Repo<StockIn>().Update(entity);
        await _uow.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _uow.Repo<StockIn>().GetByIdAsync(id, ct);
        if (entity is null) return false;

        entity.DeletedAt = DateTime.UtcNow;
        _uow.Repo<StockIn>().Update(entity);
        await _uow.SaveChangesAsync(ct);

        return true;
    }

    private async Task EnsureProductPackageAsync(Guid productId, Guid packageId, CancellationToken ct)
    {
        var package = await _uow.Repo<ProductPackage>().Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == packageId, ct);

        if (package is null)
            throw new AppException("ProductPackage topilmadi.", 404, "not_found");

        if (package.ProductId != productId)
            throw new AppException("ProductPackage tanlangan Product ga tegishli emas.", 400, "validation_error");
    }
}
