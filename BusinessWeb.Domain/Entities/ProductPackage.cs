namespace BusinessWeb.Domain.Entities;

public class ProductPackage : BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
