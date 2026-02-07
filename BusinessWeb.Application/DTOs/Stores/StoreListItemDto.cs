namespace BusinessWeb.Application.DTOs.Stores;

public class StoreListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public bool IsActive { get; set; }
}
