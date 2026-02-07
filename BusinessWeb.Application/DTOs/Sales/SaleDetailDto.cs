using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Application.DTOs.Sales;

public class SaleDetailDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public PaymentType PaymentType { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SaleLineDetailDto> Lines { get; set; } = new();
}
