using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Application.DTOs.Sales;

public class SaleListItemDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public PaymentType PaymentType { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}
