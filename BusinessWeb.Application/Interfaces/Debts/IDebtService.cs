using BusinessWeb.Application.DTOs.Debts;

namespace BusinessWeb.Application.Interfaces.Debts;

public interface IDebtService
{
    Task<List<DebtListItemDto>> GetAllAsync(CancellationToken ct = default);
    Task<DebtDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<DebtPayResultDto> PayAsync(DebtPayRequestDto dto, CancellationToken ct = default);
}
