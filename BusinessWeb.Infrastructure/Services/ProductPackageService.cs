using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessWeb.Application.DTOs.ProductPackages;
using BusinessWeb.Application.Exceptions;
using BusinessWeb.Application.Interfaces;
using BusinessWeb.Application.Interfaces.ProductPackages;
using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Infrastructure.Services;

public class ProductPackageService : IProductPackageService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ProductPackageService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public Task<List<ProductPackageListItemDto>> GetAllAsync(CancellationToken ct = default)
        => _uow.Repo<ProductPackage>().Query()
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<ProductPackageListItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

    public Task<ProductPackageDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _uow.Repo<ProductPackage>().Query()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<ProductPackageDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);

    public async Task<ProductPackageDetailDto> CreateAsync(ProductPackageCreateDto dto, CancellationToken ct = default)
    {
        var productExists = await _uow.Repo<Product>().Query()
            .AsNoTracking()
            .AnyAsync(x => x.Id == dto.ProductId, ct);

        if (!productExists)
            throw new AppException("Product topilmadi.", 404, "not_found");

        var entity = _mapper.Map<ProductPackage>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        await _uow.Repo<ProductPackage>().AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return await GetByIdAsync(entity.Id, ct) ?? _mapper.Map<ProductPackageDetailDto>(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, ProductPackageUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Repo<ProductPackage>().GetByIdAsync(id, ct);
        if (entity is null) return false;

        var productExists = await _uow.Repo<Product>().Query()
            .AsNoTracking()
            .AnyAsync(x => x.Id == dto.ProductId, ct);

        if (!productExists)
            throw new AppException("Product topilmadi.", 404, "not_found");

        _mapper.Map(dto, entity);
        _uow.Repo<ProductPackage>().Update(entity);
        await _uow.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _uow.Repo<ProductPackage>().GetByIdAsync(id, ct);
        if (entity is null) return false;

        entity.DeletedAt = DateTime.UtcNow;
        _uow.Repo<ProductPackage>().Update(entity);
        await _uow.SaveChangesAsync(ct);

        return true;
    }
}
