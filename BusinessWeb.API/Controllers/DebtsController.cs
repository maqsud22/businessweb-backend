using BusinessWeb.Application.DTOs.Debts;
using BusinessWeb.Application.Interfaces.Debts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Operator")]
public class DebtsController : ControllerBase
{
    private readonly IDebtService _service;

    public DebtsController(IDebtService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<DebtListItemDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DebtDetailDto>> GetById(Guid id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("pay")]
    public async Task<ActionResult<DebtPayResultDto>> Pay([FromBody] DebtPayRequestDto dto, CancellationToken ct)
        => Ok(await _service.PayAsync(dto, ct));
}
