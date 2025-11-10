// Services/IPatientService.cs
using HospitalWebApi.DTOs;
using AutoMapper;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;
public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync();
    Task<PatientDto?> GetByIdAsync(int id);
    Task<PatientDto> CreateAsync(PatientDto dto);
    Task<bool> UpdateAsync(int id, PatientDto dto);
    Task<bool> DeleteAsync(int id);
    Task<(IEnumerable<PatientDto> Patients, int TotalCount)> GetPagedAsync(string? search, int page, int pageSize);
    Task<IEnumerable<PatientDto>> SearchAsync(string term);



}
public class PatientService : IPatientService
{
    private readonly HospitalContext _context;
    private readonly IMapper _mapper;

    public PatientService(HospitalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync()
    {
        var list = await _context.Patients.ToListAsync();
        return _mapper.Map<IEnumerable<PatientDto>>(list);
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        return patient == null ? null : _mapper.Map<PatientDto>(patient);
    }

    public async Task<PatientDto> CreateAsync(PatientDto dto)
    {
        var patient = _mapper.Map<Patient>(dto);
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task<bool> UpdateAsync(int id, PatientDto dto)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return false;

        _mapper.Map(dto, patient);
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return false;

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<(IEnumerable<PatientDto> Patients, int TotalCount)> GetPagedAsync(string? search, int page, int pageSize)
    {
        var query = _context.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(p =>
                p.PatientName.ToLower().Contains(search) ||
                p.Phone.ToLower().Contains(search) ||
                p.Address.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync();

        var patients = await query
            .OrderBy(p => p.PatientName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<PatientDto>>(patients);

        return (result, totalCount);
    }

    public async Task<IEnumerable<PatientDto>> SearchAsync(string term)
    {
        term = term.ToLower();

        var patients = await _context.Patients
            .Where(p => p.PatientName.ToLower().Contains(term) ||
                        p.Phone.Contains(term))
            .OrderBy(p => p.PatientName)
            .Take(20)
            .ToListAsync();

        return _mapper.Map<IEnumerable<PatientDto>>(patients);
    }

}
