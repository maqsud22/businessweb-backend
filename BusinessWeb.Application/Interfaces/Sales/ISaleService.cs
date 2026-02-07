using BusinessWeb.Application.DTOs.Sales;

namespace BusinessWeb.Application.Interfaces.Sales;

public interface ISaleService
{
    Task<List<SaleListItemDto>> GetAllAsync(CancellationToken ct = default);
    Task<SaleDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<SaleResultDto> CreateAsync(CreateSaleDto dto, CancellationToken ct = default);
}
