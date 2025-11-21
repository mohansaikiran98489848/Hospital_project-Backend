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
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _svc;
        private readonly HospitalContext _context;

        public VisitsController(IVisitService svc, HospitalContext context)
        {
            _svc = svc;
            _context = context;
        }

        // Doctor: get visits for logged-in doctor
        [Authorize(Roles = "Doctor")]
        [HttpGet("my")]
        public async Task<IActionResult> MyVisits()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorName == username);
            if (doctor == null) return Unauthorized();

            var list = await _svc.GetByDoctorAsync(doctor.DoctorId);
            return Ok(list);
        }

        // Get visits by patient (Reception/Admin)
        [Authorize(Roles = "Reception,Admin,Doctor")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> ByPatient(int patientId)
        {
            var list = await _svc.GetByPatientAsync(patientId);
            return Ok(list);
        }

        // Get visit by id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var v = await _svc.GetByIdAsync(id);
            if (v == null) return NotFound();
            return Ok(v);
        }
    }
}
