namespace BusinessWeb.Application.DTOs.Stores;

public class StoreDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
