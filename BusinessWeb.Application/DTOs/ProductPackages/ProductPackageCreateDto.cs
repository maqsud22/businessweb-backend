namespace BusinessWeb.Application.DTOs.ProductPackages;

public class ProductPackageCreateDto
{
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }
    public Guid ProductId { get; set; }
}
