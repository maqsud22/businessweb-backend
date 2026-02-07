namespace BusinessWeb.Application.DTOs.Reports;

public class SalesSummaryDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal TotalSales { get; set; }
    public decimal CashSales { get; set; }
    public decimal DebtSales { get; set; }
    public decimal PartialSales { get; set; }
    public decimal DebtPayments { get; set; }
    public int SaleCount { get; set; }
}
