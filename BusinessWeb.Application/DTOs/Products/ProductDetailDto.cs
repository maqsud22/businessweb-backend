using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Application.DTOs.Products;

public class ProductDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public UnitType Unit { get; set; }
    public decimal DefaultPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
