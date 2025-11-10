// Controllers/PatientsController.cs
using Microsoft.AspNetCore.Mvc;
using HospitalWebApi.DTOs;

[Route("api/[controller]")]
[ApiController]

public class PatientsController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientsController(IPatientService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] string? search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
    {
        var (patients, totalCount) = await _service.GetPagedAsync(search, page, pageSize);

        return Ok(new
        {
            totalCount,
            patients
        });
    }
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<PatientDto>>> Search([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Ok(new List<PatientDto>());

        var patients = await _service.SearchAsync(term);
        return Ok(patients);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> Get(int id)
    {
        var patient = await _service.GetByIdAsync(id);
        if (patient == null) return NotFound();
        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create(PatientDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.PatientId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, PatientDto dto)
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
