using BusinessWeb.Application.DTOs.StockIns;
using BusinessWeb.Application.Interfaces.StockIns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Operator")]
public class StockInsController : ControllerBase
{
    private readonly IStockInService _service;

    public StockInsController(IStockInService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<StockInListItemDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StockInDetailDto>> GetById(Guid id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StockInDetailDto>> Create([FromBody] StockInCreateDto dto, CancellationToken ct)
        => Ok(await _service.CreateAsync(dto, ct));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StockInUpdateDto dto, CancellationToken ct)
        => (await _service.UpdateAsync(id, dto, ct)) ? NoContent() : NotFound();

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDelete(Guid id, CancellationToken ct)
        => (await _service.SoftDeleteAsync(id, ct)) ? NoContent() : NotFound();
}
