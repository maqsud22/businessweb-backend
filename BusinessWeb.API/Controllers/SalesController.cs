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

    [HttpPost]
    public async Task<ActionResult<SaleResultDto>> Create([FromBody] CreateSaleDto dto, CancellationToken ct)
        => Ok(await _service.CreateAsync(dto, ct));
}
