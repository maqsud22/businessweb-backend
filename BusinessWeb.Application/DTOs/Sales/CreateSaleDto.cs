using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Application.DTOs.Sales;

public class CreateSaleDto
{
    public Guid StoreId { get; set; }
    public PaymentType PaymentType { get; set; }

    /// <summary>
    /// Cash: totalga teng bo'lishi kerak
    /// Debt: 0 bo'lishi kerak
    /// Partial: 0 < paid < total
    /// </summary>
    public decimal PaidAmount { get; set; }

    public List<CreateSaleLineDto> Lines { get; set; } = new();
}
