using BusinessWeb.Application.DTOs.StockIns;

namespace BusinessWeb.Application.Interfaces.StockIns;

public interface IStockInService
{
    Task<List<StockInListItemDto>> GetAllAsync(CancellationToken ct = default);
    Task<StockInDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<StockInDetailDto> CreateAsync(StockInCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, StockInUpdateDto dto, CancellationToken ct = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default);
}
