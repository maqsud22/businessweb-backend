namespace BusinessWeb.Application.DTOs.ProductPackages;

public class ProductPackageUpdateDto
{
    public string Name { get; set; } = null!;
    public decimal Multiplier { get; set; }
    public Guid ProductId { get; set; }
}
