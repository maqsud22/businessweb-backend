using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Domain.Entities;

public class Sale : BaseEntity
{
    public Guid StoreId { get; set; }
    public Store Store { get; set; } = null!;

    public PaymentType PaymentType { get; set; }
    public decimal TotalAmount { get; set; }

    public ICollection<SaleLine> Lines { get; set; } = new List<SaleLine>();
}
