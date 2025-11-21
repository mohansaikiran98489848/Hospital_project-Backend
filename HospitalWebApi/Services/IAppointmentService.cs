using AutoMapper;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HospitalWebApi.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> ScheduleAsync(AppointmentDto dto);
        Task<bool> CancelAsync(int id);
        Task<bool> CompleteAsync(int id, int? consultationId = null);
        Task<IEnumerable<AppointmentDto>> GetByDoctorAsync(int doctorId);
        Task<DoctorAppointmentSummaryDto> GetDoctorSummaryAsync(int doctorId);
        Task<AppointmentDto?> GetByIdAsync(int id);
    }
}


namespace HospitalWebApi.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HospitalContext _context;
        private readonly IMapper _mapper;

        public AppointmentService(HospitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> ScheduleAsync(AppointmentDto dto)
        {
            var entity = _mapper.Map<Appointment>(dto);
            _context.Appointments.Add(entity);
            await _context.SaveChangesAsync();

            // Map back (include names for convenience)
            var result = _mapper.Map<AppointmentDto>(entity);
            result.DoctorName = (await _context.Doctors.FindAsync(entity.DoctorId))?.DoctorName;
            result.PatientName = (await _context.Patients.FindAsync(entity.PatientId))?.PatientName;
            return result;
        }

        public async Task<bool> CancelAsync(int id)
        {
            var a = await _context.Appointments.FindAsync(id);
            if (a == null) return false;
            a.Status = "Cancelled";
            _context.Appointments.Update(a);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteAsync(int id, int? consultationId = null)
        {
            var a = await _context.Appointments.FindAsync(id);
            if (a == null) return false;

            a.Status = "Completed";
            if (consultationId.HasValue) a.ConsultationId = consultationId.Value;
            _context.Appointments.Update(a);
            await _context.SaveChangesAsync();

            // Create Visit when appointment is completed and has a consultation
            if (a.ConsultationId.HasValue)
            {
                var visitCount = await _context.Visits
                    .CountAsync(v => v.PatientId == a.PatientId && v.DoctorId == a.DoctorId);

                var visit = new Visit
                {
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    ConsultationId = a.ConsultationId.Value,
                    VisitType = a.VisitType ?? "Normal",
                    VisitNumber = visitCount + 1,
                    VisitDate = DateTime.UtcNow,
                    Notes = a.Notes
                };

                _context.Visits.Add(visit);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<AppointmentDto>> GetByDoctorAsync(int doctorId)
        {
            var list = await _context.Appointments
                .AsNoTracking()
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.ScheduledAt)
                .ToListAsync();

            var dtoList = _mapper.Map<IEnumerable<AppointmentDto>>(list);

            // populate patient/doctor names (optional)
            var patientIds = dtoList.Select(d => d.PatientId).Distinct().ToList();
            var patients = await _context.Patients.Where(p => patientIds.Contains(p.PatientId))
                                                 .ToDictionaryAsync(p => p.PatientId, p => p.PatientName);

            foreach (var d in dtoList)
            {
                if (patients.TryGetValue(d.PatientId, out var name)) d.PatientName = name;
                d.DoctorName = (await _context.Doctors.FindAsync(d.DoctorId))?.DoctorName;
            }

            return dtoList;
        }

        public async Task<DoctorAppointmentSummaryDto> GetDoctorSummaryAsync(int doctorId)
        {
            var all = _context.Appointments.Where(a => a.DoctorId == doctorId);

            var total = await all.CountAsync();
            var pending = await all.CountAsync(a => a.Status == "Pending");
            var completed = await all.CountAsync(a => a.Status == "Completed");
            var fresh = await all.CountAsync(a => a.VisitType == "Fresh");
            var old = await all.CountAsync(a => a.VisitType != "Fresh");

            return new DoctorAppointmentSummaryDto
            {
                DoctorId = doctorId,
                Total = total,
                Pending = pending,
                Completed = completed,
                Fresh = fresh,
                Old = old
            };
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var a = await _context.Appointments
                .AsNoTracking()
                .Include(x => x.Patient)
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.AppointmentId == id);

            return a == null ? null : _mapper.Map<AppointmentDto>(a);
        }

    }
}
