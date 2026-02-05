namespace BusinessWeb.Domain.Entities;

public class Debt : BaseEntity
{
    public Guid SaleId { get; set; }
    public decimal Total { get; set; }
    public decimal Paid { get; set; }
    public bool IsClosed { get; set; }

    public ICollection<DebtPayment> Payments { get; set; } = new List<DebtPayment>();
}
