namespace BusinessWeb.Domain.Entities;

public class DebtPayment : BaseEntity
{
    public Guid DebtId { get; set; }
    public Debt Debt { get; set; } = null!;

    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
