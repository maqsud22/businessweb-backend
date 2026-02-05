namespace BusinessWeb.Domain.Entities;

public class Store : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
