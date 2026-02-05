using BusinessWeb.Application.DTOs.Products;

namespace BusinessWeb.Application.Interfaces.Products;

public interface IProductService
{
    Task<List<ProductListItemDto>> GetAllAsync(CancellationToken ct = default);
    Task<ProductDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProductDetailDto> CreateAsync(ProductCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto, CancellationToken ct = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default);
}
