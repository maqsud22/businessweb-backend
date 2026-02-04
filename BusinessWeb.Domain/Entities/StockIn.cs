namespace BusinessWeb.Domain.Entities;

public class StockIn : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Guid ProductPackageId { get; set; }
    public ProductPackage ProductPackage { get; set; } = null!;

    public decimal Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public DateTime Date { get; set; }
}
