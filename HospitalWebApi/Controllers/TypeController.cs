using HospitalWebApi.DTOs;
using HospitalWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeController : ControllerBase
    {
        private readonly ITypeService _service;

        public TypeController(ITypeService service)
        {
            _service = service;
        }

        // Parents dropdown / full list (no pagination)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // Paged + search
        // GET /api/type/paged?page=1&pageSize=10&search=card
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 10;

            var result = await _service.GetPagedAsync(page, pageSize, search);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TypeDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TypeDto dto)
        {
            if (id != dto.Id) return BadRequest("Route id and body id mismatch.");
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _service.DeleteAsync(id);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
