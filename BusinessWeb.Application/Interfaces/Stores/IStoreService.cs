using BusinessWeb.Application.DTOs.Stores;

namespace BusinessWeb.Application.Interfaces.Stores;

public interface IStoreService
{
    Task<List<StoreListItemDto>> GetAllAsync(CancellationToken ct = default);
    Task<StoreDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<StoreDetailDto> CreateAsync(StoreCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, StoreUpdateDto dto, CancellationToken ct = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken ct = default);
}
