namespace BusinessWeb.Application.DTOs.ProductPackages;

public class ProductPackageListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
}
