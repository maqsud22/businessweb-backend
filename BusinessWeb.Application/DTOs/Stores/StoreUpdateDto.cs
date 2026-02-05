namespace BusinessWeb.Application.DTOs.Stores;

public class StoreUpdateDto
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public bool IsActive { get; set; }
}
