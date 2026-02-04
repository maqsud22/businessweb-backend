namespace BusinessWeb.Application.DTOs.StockIns;

public class StockInUpdateDto
{
    public Guid ProductId { get; set; }
    public Guid ProductPackageId { get; set; }
    public decimal Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public DateTime Date { get; set; }
}
