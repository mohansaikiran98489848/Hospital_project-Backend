using AutoMapper;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace HospitalWebApi.Services
{
    public interface IVisitService
    {
        Task<VisitDto> GetByIdAsync(int id);
        Task<IEnumerable<VisitDto>> GetByPatientAsync(int patientId);
        Task<IEnumerable<VisitDto>> GetByDoctorAsync(int doctorId);
        Task<VisitDto> CreateAsync(VisitDto dto);
    }
}

namespace HospitalWebApi.Services
{
    public class VisitService : IVisitService
    {
        private readonly HospitalContext _context;
        private readonly IMapper _mapper;

        public VisitService(HospitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VisitDto> CreateAsync(VisitDto dto)
        {
            var entity = _mapper.Map<Visit>(dto);
            _context.Visits.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<VisitDto>(entity);
        }

        public async Task<IEnumerable<VisitDto>> GetByDoctorAsync(int doctorId)
        {
            var list = await _context.Visits
                .AsNoTracking()
                .Where(v => v.DoctorId == doctorId)
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<VisitDto>>(list);
        }

        public async Task<IEnumerable<VisitDto>> GetByPatientAsync(int patientId)
        {
            var list = await _context.Visits
                .AsNoTracking()
                .Where(v => v.PatientId == patientId)
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<VisitDto>>(list);
        }

        public async Task<VisitDto> GetByIdAsync(int id)
        {
            var v = await _context.Visits.FindAsync(id);
            return v == null ? null! : _mapper.Map<VisitDto>(v);
        }
    }
}
