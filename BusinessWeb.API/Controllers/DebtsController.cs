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

    [HttpPost("pay")]
    public async Task<ActionResult<DebtPayResultDto>> Pay([FromBody] DebtPayRequestDto dto, CancellationToken ct)
        => Ok(await _service.PayAsync(dto, ct));
}
