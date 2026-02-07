namespace BusinessWeb.Application.DTOs.Debts;

public class DebtPayResultDto
{
    public Guid DebtId { get; set; }
    public Guid PaymentId { get; set; }
    public decimal Total { get; set; }
    public decimal Paid { get; set; }
    public decimal Remaining { get; set; }
    public bool IsClosed { get; set; }
}
