using HospitalWebApi.DTOs;
using HospitalWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _service;
    public DepartmentsController(IDepartmentService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> Get(int id)
    {
        var dep = await _service.GetByIdAsync(id);
        if (dep == null) return NotFound();
        return Ok(dep);
    }

    [HttpGet("paged")]
    public async Task<ActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        => Ok(await _service.GetPagedAsync(page, pageSize, search));

    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> Create(DepartmentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.DepartmentName))
            return BadRequest("Department name is required.");

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.DepartmentId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, DepartmentDto dto)
    {
        var ok = await _service.UpdateAsync(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var (ok, blocked) = await _service.DeleteAsync(id);
        if (!ok && !string.IsNullOrEmpty(blocked)) return Conflict(blocked);
        if (!ok) return NotFound();
        return NoContent();
    }

    // ✅ Async name check (for validator)
    [HttpGet("check-name")]
    public async Task<ActionResult<bool>> CheckName([FromQuery] string name, [FromQuery] int? id = null)
    {
        var exists = await _service.ExistsByNameAsync(name, id);
        return Ok(exists);
    }
}
