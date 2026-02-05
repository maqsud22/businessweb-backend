using BusinessWeb.Application.DTOs.ProductPackages;

namespace BusinessWeb.Application.Interfaces.ProductPackages;

public interface IProductPackageService
{
    Task<List<ProductPackageListItemDto>> GetAllAsync(CancellationToken ct = default);
    Task<ProductPackageDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProductPackageDetailDto> CreateAsync(ProductPackageCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, ProductPackageUpdateDto dto, CancellationToken ct = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default);
}
