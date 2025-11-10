using Microsoft.AspNetCore.Mvc;
using HospitalWebApi.DTOs;

[Route("api/[controller]")]
[ApiController]
public class ServiceReceiptsController : ControllerBase
{
    private readonly IServiceReceiptService _service;
    public ServiceReceiptsController(IServiceReceiptService service) => _service = service;

    [HttpGet] public async Task<ActionResult<IEnumerable<ServiceReceiptDto>>> GetAll() => Ok(await _service.GetAllAsync());
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceReceiptDto>> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }
    [HttpPost]
    public async Task<ActionResult<ServiceReceiptDto>> Create(ServiceReceiptDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.ServiceReceiptId }, created);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, ServiceReceiptDto dto)
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