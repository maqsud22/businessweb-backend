using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Application.DTOs.Sales;

public class SaleResultDto
{
    public Guid SaleId { get; set; }
    public Guid StoreId { get; set; }

    public PaymentType PaymentType { get; set; }
    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }        // faqat requestdan kelgan
    public decimal RemainingAmount { get; set; }   // total - paid

    public Guid? DebtId { get; set; }
    public bool? DebtIsClosed { get; set; }
}
