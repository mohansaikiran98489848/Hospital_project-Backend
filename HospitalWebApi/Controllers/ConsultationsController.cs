using Microsoft.AspNetCore.Mvc;
using HospitalWebApi.DTOs;

[Route("api/[controller]")]
[ApiController]
public class ConsultationsController : ControllerBase
{
    private readonly IConsultationService _service;
    public ConsultationsController(IConsultationService service) => _service = service;

    [HttpGet] public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetAll() => Ok(await _service.GetAllAsync());
    [HttpGet("{id}")]
    public async Task<ActionResult<ConsultationDto>> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }
    [HttpPost]
    public async Task<ActionResult<ConsultationDto>> Create(ConsultationDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.ConsultationId }, created);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, ConsultationDto dto)
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