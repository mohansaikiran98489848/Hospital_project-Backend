using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using HospitalWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HospitalWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _svc;
        private readonly HospitalContext _context;

        public AppointmentsController(IAppointmentService svc, HospitalContext context)
        {
            _svc = svc;
            _context = context;
        }

        // Helper: get logged in doctor id by Username == ClaimTypes.Name
        private async Task<int?> GetLoggedInDoctorId()
        {
            var username =
                User.FindFirstValue(ClaimTypes.Name) ??
                User.FindFirstValue("unique_name") ??
                User.FindFirstValue(ClaimTypes.GivenName);

            if (string.IsNullOrEmpty(username))
                return null;

            var doctor = await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DoctorName == username);

            return doctor?.DoctorId;
        }


        // Doctor: get only his appointments
        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var doctorId = await GetLoggedInDoctorId();
            if (doctorId == null) return Unauthorized("Doctor not found");

            var list = await _svc.GetByDoctorAsync(doctorId.Value);
            return Ok(list);
        }

        // Doctor: summary
        [Authorize(Roles = "Doctor")]
        [HttpGet("my/summary")]
        public async Task<IActionResult> MySummary()
        {
            var doctorId = await GetLoggedInDoctorId();
            if (doctorId == null) return Unauthorized("Doctor not found");

            var summary = await _svc.GetDoctorSummaryAsync(doctorId.Value);
            return Ok(summary);
        }

        // Doctor completes his appointment (only his own)
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(int id, [FromQuery] int? consultationId)
        {
            var doctorId = await GetLoggedInDoctorId();
            if (doctorId == null) return Unauthorized("Doctor not found");

            var appt = await _svc.GetByIdAsync(id);
            if (appt == null || appt.DoctorId != doctorId.Value) return Unauthorized("Not allowed");

            var ok = await _svc.CompleteAsync(id, consultationId);
            if (!ok) return NotFound();
            return NoContent();
        }

        // Reception + Admin schedule
        [Authorize(Roles = "Reception,Admin")]
        [HttpPost("schedule")]
        public async Task<IActionResult> Schedule([FromBody] AppointmentDto dto)
        {
            var created = await _svc.ScheduleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.AppointmentId }, created);
        }

        // Get appointment by id (Admin/Reception/Doctor can view if needed)
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var a = await _svc.GetByIdAsync(id);
            if (a == null) return NotFound();
            return Ok(a);
        }
    }
}
