using HospitalWebApi.DTOs;
using HospitalWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceptionController : ControllerBase
    {
        private readonly IReceptionService _receptionService;

        public ReceptionController(IReceptionService receptionService)
        {
            _receptionService = receptionService;
        }

        // 🔹 POST: Register patient + Create Bill
        [HttpPost("register")]
        public async Task<IActionResult> RegisterOrBill([FromBody] ReceptionDto dto)
        {
            try
            {
                var result = await _receptionService.RegisterOrBillAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
