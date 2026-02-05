using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessWeb.Application.DTOs.Products;
using BusinessWeb.Application.Interfaces;
using BusinessWeb.Application.Interfaces.Products;
using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public Task<List<ProductListItemDto>> GetAllAsync(CancellationToken ct = default)
        => _uow.Repo<Product>().Query()
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<ProductListItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

    public Task<ProductDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _uow.Repo<Product>().Query()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);

    public async Task<ProductDetailDto> CreateAsync(ProductCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<Product>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        await _uow.Repo<Product>().AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<ProductDetailDto>(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Repo<Product>().GetByIdAsync(id, ct);
        if (entity is null) return false;

        _mapper.Map(dto, entity);
        _uow.Repo<Product>().Update(entity);
        await _uow.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _uow.Repo<Product>().GetByIdAsync(id, ct);
        if (entity is null) return false;

        entity.DeletedAt = DateTime.UtcNow;
        _uow.Repo<Product>().Update(entity);
        await _uow.SaveChangesAsync(ct);

        return true;
    }
}
