namespace BusinessWeb.Application.DTOs.Sales;

public class SaleLineDetailDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid ProductPackageId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}
