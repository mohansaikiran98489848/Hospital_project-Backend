using HospitalWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BillReceiptsController : ControllerBase
{
    private readonly IBillReceiptService _service;

    public BillReceiptsController(IBillReceiptService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillReceiptDto>>> GetAll([FromQuery] int? billHeaderId)
    {
        var receipts = await _service.GetAllAsync(billHeaderId);
        return Ok(receipts);
    }

    [HttpPost]
    public async Task<ActionResult<BillReceiptDto>> Create(BillReceiptDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById),
            new { billHeaderId = created.BillHeaderId, id = created.BillReceiptId },
            created);
    }

    [HttpGet("{billHeaderId}/billreceipts/{id}")]
    public async Task<ActionResult<BillReceiptDto>> GetById(int billHeaderId, int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null || entity.BillHeaderId != billHeaderId)
            return NotFound();

        return Ok(entity);
    }

    [HttpPut("{billHeaderId}/billreceipts/{id}")]
    public async Task<IActionResult> Update(int billHeaderId, int id, BillReceiptDto dto)
    {
        if (id != dto.BillReceiptId)
            return BadRequest("ID mismatch between route and body.");

        dto.BillHeaderId = billHeaderId;

        var existing = await _service.GetByIdAsync(id);
        if (existing == null || existing.BillHeaderId != billHeaderId)
            return NotFound();

        var success = await _service.UpdateAsync(id, dto);
        if (!success) return BadRequest("Update failed.");

        return NoContent();
    }

    [HttpDelete("{billHeaderId}/billreceipts/{id}")]
    public async Task<IActionResult> Delete(int billHeaderId, int id)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null || existing.BillHeaderId != billHeaderId)
            return NotFound();

        var success = await _service.DeleteAsync(id);
        if (!success) return BadRequest("Delete failed.");

        return NoContent();
    }
}
