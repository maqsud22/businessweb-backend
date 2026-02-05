using BusinessWeb.Application.DTOs.Sales;
using BusinessWeb.Application.Interfaces.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Operator")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _service;

    public SalesController(ISaleService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<SaleListItemDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SaleDetailDto>> GetById(Guid id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<SaleResultDto>> Create([FromBody] CreateSaleDto dto, CancellationToken ct)
        => Ok(await _service.CreateAsync(dto, ct));
}
