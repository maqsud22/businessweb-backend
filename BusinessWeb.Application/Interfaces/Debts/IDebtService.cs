using BusinessWeb.Application.DTOs.Debts;

namespace BusinessWeb.Application.Interfaces.Debts;

public interface IDebtService
{
    Task<DebtPayResultDto> PayAsync(DebtPayRequestDto dto, CancellationToken ct = default);
}
