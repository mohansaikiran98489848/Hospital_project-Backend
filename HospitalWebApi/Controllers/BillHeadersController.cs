using Microsoft.AspNetCore.Mvc;
using HospitalWebApi.DTOs;

[Route("api/[controller]")]
[ApiController]
public class BillHeadersController : ControllerBase
{
    private readonly IBillHeaderService _service;
    public BillHeadersController(IBillHeaderService service) => _service = service;

    [HttpGet] public async Task<ActionResult<IEnumerable<BillHeaderDto>>> GetAll() => Ok(await _service.GetAllAsync());
    [HttpGet("{id}")]
    public async Task<ActionResult<BillHeaderDto>> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }
    [HttpGet("{headerId}/receipts")]
    public async Task<ActionResult<IEnumerable<BillReceiptDto>>> GetReceiptsByHeader(int headerId, [FromServices] IBillReceiptService receiptService)
    {
        var receipts = await receiptService.GetAllAsync(headerId);
        return Ok(receipts);
    }

    [HttpPost]
    public async Task<ActionResult<BillHeaderDto>> Create(BillHeaderDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.BillHeaderId }, created);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, BillHeaderDto dto)
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
