using HospitalWebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _service;
    public DoctorsController(IDoctorService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        var result = await _service.GetPagedAsync(page, pageSize, search);
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDto>> Get(int id)
    {
        var doctor = await _service.GetByIdAsync(id);
        if (doctor == null) return NotFound();
        return Ok(doctor);
    }


    [HttpGet("by-department/{departmentId}")]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetByDepartment(int departmentId)
    {
        var result = await _service.GetByDepartmentAsync(departmentId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> Create(DoctorDto dto)
    {
        if (dto.TypeId == null || dto.TypeId <= 0)
            return BadRequest("TypeId is required.");

        if (dto.DepartmentId == null || dto.DepartmentId <= 0)
            return BadRequest("DepartmentId is required.");

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.DoctorId }, created);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, DoctorDto dto)
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
