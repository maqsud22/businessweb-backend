namespace BusinessWeb.Application.DTOs.Debts;

public class DebtPayRequestDto
{
    public Guid DebtId { get; set; }
    public decimal Amount { get; set; }
    public DateTime? Date { get; set; }
}
