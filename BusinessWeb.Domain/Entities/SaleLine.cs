namespace BusinessWeb.Domain.Entities;

public class SaleLine : BaseEntity
{
    public Guid SaleId { get; set; }
    public Sale Sale { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Guid ProductPackageId { get; set; }
    public ProductPackage ProductPackage { get; set; } = null!;

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}
