using BusinessWeb.Application.DTOs.Reports;
using BusinessWeb.Application.Interfaces.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Operator")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _service;

    public ReportsController(IReportService service) => _service = service;

    [HttpGet("daily")]
    public async Task<ActionResult<SalesSummaryDto>> Daily([FromQuery] DateTime? date, CancellationToken ct)
        => Ok(await _service.GetDailyAsync(date, ct));

    [HttpGet("monthly")]
    public async Task<ActionResult<SalesSummaryDto>> Monthly([FromQuery] DateTime? date, CancellationToken ct)
        => Ok(await _service.GetMonthlyAsync(date, ct));

    [HttpGet("product/{id:guid}")]
    public async Task<ActionResult<ProductReportDto>> Product([FromRoute] Guid id, CancellationToken ct)
        => Ok(await _service.GetProductAsync(id, ct));

    [HttpGet("stock")]
    public async Task<ActionResult<IReadOnlyList<StockReportItemDto>>> Stock(CancellationToken ct)
        => Ok(await _service.GetStockAsync(ct));
}
