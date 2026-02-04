using BusinessWeb.Application.DTOs.Sales;

namespace BusinessWeb.Application.Interfaces.Sales;

public interface ISaleService
{
    Task<SaleResultDto> CreateAsync(CreateSaleDto dto, CancellationToken ct = default);
}
