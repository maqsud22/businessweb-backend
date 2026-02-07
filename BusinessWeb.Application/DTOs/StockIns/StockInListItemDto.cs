namespace BusinessWeb.Application.DTOs.StockIns;

public class StockInListItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public Guid ProductPackageId { get; set; }
    public string ProductPackageName { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public DateTime Date { get; set; }
}
