using Microsoft.AspNetCore.Mvc;
using HospitalWebApi.DTOs;

[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _service;
    public ServicesController(IServiceService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _service.GetPagedAsync(page, pageSize, search);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto>> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }
    [HttpPost]
    public async Task<ActionResult<ServiceDto>> Create(ServiceDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.ServiceId }, created);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, ServiceDto dto)
    {
        if (!await _service.UpdateAsync(id, dto)) return BadRequest();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id)) return NotFound();
        return NoContent();
    }
}