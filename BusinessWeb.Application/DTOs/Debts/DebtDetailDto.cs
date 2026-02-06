namespace BusinessWeb.Application.DTOs.Debts;

public class DebtDetailDto
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public decimal Total { get; set; }
    public decimal Paid { get; set; }
    public bool IsClosed { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<DebtPaymentDto> Payments { get; set; } = new();
}
