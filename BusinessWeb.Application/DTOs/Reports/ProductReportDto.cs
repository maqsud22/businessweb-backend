namespace BusinessWeb.Application.DTOs.Reports;

public class ProductReportDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal IncomingBaseQty { get; set; }
    public decimal SoldBaseQty { get; set; }
    public decimal InStockBaseQty { get; set; }
}
