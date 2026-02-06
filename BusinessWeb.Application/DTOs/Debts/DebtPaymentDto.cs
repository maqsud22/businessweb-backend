namespace BusinessWeb.Application.DTOs.Debts;

public class DebtPaymentDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
