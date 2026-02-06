using BusinessWeb.Application.DTOs.Reports;

namespace BusinessWeb.Application.Interfaces.Reports;

public interface IReportService
{
    Task<SalesSummaryDto> GetDailyAsync(DateTime? date, CancellationToken ct = default);
    Task<SalesSummaryDto> GetMonthlyAsync(DateTime? date, CancellationToken ct = default);
    Task<ProductReportDto> GetProductAsync(Guid productId, CancellationToken ct = default);
    Task<IReadOnlyList<StockReportItemDto>> GetStockAsync(CancellationToken ct = default);
}
