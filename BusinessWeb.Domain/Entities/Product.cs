using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;
    public UnitType Unit { get; set; }
    public decimal DefaultPrice { get; set; }
    public ICollection<ProductPackage> Packages { get; set; } = new List<ProductPackage>();
}
