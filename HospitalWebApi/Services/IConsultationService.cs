using AutoMapper;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;

public interface IConsultationService
{
    Task<IEnumerable<ConsultationDto>> GetAllAsync();
    Task<ConsultationDto?> GetByIdAsync(int id);
    Task<ConsultationDto> CreateAsync(ConsultationDto dto);
    Task<bool> UpdateAsync(int id, ConsultationDto dto);
    Task<bool> DeleteAsync(int id);
}



public class ConsultationService : IConsultationService
{
    private readonly HospitalContext _context;
    private readonly IMapper _mapper;

    public ConsultationService(HospitalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ConsultationDto>> GetAllAsync()
    {
        var consultations = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Doctor)
            .Include(c => c.Service)
            .ThenInclude(s => s.Department)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ConsultationDto>>(consultations);
    }

    public async Task<ConsultationDto?> GetByIdAsync(int id)
    {
        var consultation = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Doctor)
            .Include(c => c.Service)
            .ThenInclude(s => s.Department)
            .FirstOrDefaultAsync(c => c.ConsultationId == id);

        return consultation == null ? null : _mapper.Map<ConsultationDto>(consultation);
    }


    public async Task<ConsultationDto> CreateAsync(ConsultationDto dto)
    {
        var entity = _mapper.Map<Consultation>(dto);
        _context.Consultations.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<ConsultationDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, ConsultationDto dto)
    {
        var entity = await _context.Consultations.FindAsync(id);
        if (entity == null) return false;
        _mapper.Map(dto, entity);
        _context.Consultations.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Consultations.FindAsync(id);
        if (entity == null) return false;
        _context.Consultations.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}