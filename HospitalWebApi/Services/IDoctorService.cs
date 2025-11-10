using AutoMapper;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllAsync();
    Task<DoctorDto?> GetByIdAsync(int id);
    Task<IEnumerable<DoctorDto>> GetByDepartmentAsync(int departmentId);
    Task<DoctorDto> CreateAsync(DoctorDto dto);
    Task<bool> UpdateAsync(int id, DoctorDto dto);
    Task<bool> DeleteAsync(int id);
    Task<object> GetPagedAsync(int page, int pageSize, string? search);

}

public class DoctorService : IDoctorService
{
    private readonly HospitalContext _context;
    private readonly IMapper _mapper;

    public DoctorService(HospitalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllAsync()
    {
        var list = await _context.Doctors
            .Include(d => d.Department)
            .ToListAsync();
        return _mapper.Map<IEnumerable<DoctorDto>>(list);
    }
    public async Task<object> GetPagedAsync(int page, int pageSize, string? search)
    {
        var query = _context.Doctors
            .Include(d => d.Department)
            .AsQueryable();

        // Optional search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(d =>
                d.DoctorName.ToLower().Contains(search) ||
                (d.Department != null && d.Department.DepartmentName.ToLower().Contains(search))
            );
        }

        var totalCount = await query.CountAsync();

        var data = await query
            .OrderBy(d => d.DoctorName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var doctors = _mapper.Map<IEnumerable<DoctorDto>>(data);

        return new
        {
            totalCount,
            data = doctors
        };
    }


    public async Task<DoctorDto?> GetByIdAsync(int id)
    {
        var doctor = await _context.Doctors
            .Include(d => d.Department)
            .FirstOrDefaultAsync(d => d.DoctorId == id);
        return doctor == null ? null : _mapper.Map<DoctorDto>(doctor);
    }

    // ✅ Get doctors by department
    public async Task<IEnumerable<DoctorDto>> GetByDepartmentAsync(int departmentId)
    {
        var doctors = await _context.Doctors
            .Where(d => d.DepartmentId == departmentId)
            .Include(d => d.Department)
            .ToListAsync();
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<DoctorDto> CreateAsync(DoctorDto dto)
    {
        var doctor = _mapper.Map<Doctor>(dto);

        // ✅ Prevent EF from inserting new Department or Type
        doctor.Department = null;
        doctor.Type = null;

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        return _mapper.Map<DoctorDto>(doctor);
    }


    public async Task<bool> UpdateAsync(int id, DoctorDto dto)
    {
        var doctor = await _context.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.DoctorId == id);
        if (doctor == null) return false;

        var updatedDoctor = _mapper.Map<Doctor>(dto);
        updatedDoctor.DoctorId = id;
        updatedDoctor.Department = null;
        updatedDoctor.Type = null;

        _context.Entry(updatedDoctor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }



    public async Task<bool> DeleteAsync(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return false;
        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
        return true;
    }
}
