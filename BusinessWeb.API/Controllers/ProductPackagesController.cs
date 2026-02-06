using BusinessWeb.Application.DTOs.ProductPackages;
using BusinessWeb.Application.Interfaces.ProductPackages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Operator")]
public class ProductPackagesController : ControllerBase
{
    private readonly IProductPackageService _service;

    public ProductPackagesController(IProductPackageService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<ProductPackageListItemDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductPackageDetailDto>> GetById(Guid id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductPackageDetailDto>> Create([FromBody] ProductPackageCreateDto dto, CancellationToken ct)
        => Ok(await _service.CreateAsync(dto, ct));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductPackageUpdateDto dto, CancellationToken ct)
        => (await _service.UpdateAsync(id, dto, ct)) ? NoContent() : NotFound();

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDelete(Guid id, CancellationToken ct)
        => (await _service.SoftDeleteAsync(id, ct)) ? NoContent() : NotFound();
}
