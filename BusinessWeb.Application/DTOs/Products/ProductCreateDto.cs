using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Application.DTOs.Products;

public class ProductCreateDto
{
    public string Name { get; set; } = null!;
    public UnitType Unit { get; set; }
    public decimal DefaultPrice { get; set; }
}
