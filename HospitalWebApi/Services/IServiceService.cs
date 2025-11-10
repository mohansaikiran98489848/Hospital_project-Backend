using AutoMapper;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;

public interface IServiceService
{
    Task<IEnumerable<ServiceDto>> GetAllAsync();
    Task<ServiceDto?> GetByIdAsync(int id);
    Task<ServiceDto> CreateAsync(ServiceDto dto);
    Task<bool> UpdateAsync(int id, ServiceDto dto);
    Task<bool> DeleteAsync(int id);
    Task<object> GetPagedAsync(int page, int pageSize, string? search);

}



public class ServiceService : IServiceService
{
    private readonly HospitalContext _context;
    private readonly IMapper _mapper;

    public ServiceService(HospitalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllAsync()
    {
        var services = await _context.Services
            .Include(s => s.Type)
            .Include(s => s.Department)
            .Include(s => s.Doctor)
            .Include(s => s.Outsource)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    public async Task<ServiceDto?> GetByIdAsync(int id)
    {
        var service = await _context.Services
            .Include(s => s.Type)
            .Include(s => s.Department)
            .Include(s => s.Doctor)
            .Include(s => s.Outsource)
            .FirstOrDefaultAsync(s => s.ServiceId == id);

        return service == null ? null : _mapper.Map<ServiceDto>(service);
    }


    public async Task<ServiceDto> CreateAsync(ServiceDto dto)
    {
        var entity = _mapper.Map<Service>(dto);
        _context.Services.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<ServiceDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, ServiceDto dto)
    {
        var entity = await _context.Services.FindAsync(id);
        if (entity == null) return false;

        // prevent key overwrite
        dto.ServiceId = id;

        _mapper.Map(dto, entity);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Services.FindAsync(id);
        if (entity == null) return false;
        _context.Services.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<object> GetPagedAsync(int page, int pageSize, string? search)
    {
        var query = _context.Services
            .Include(s => s.Type)
            .Include(s => s.Department)
            .Include(s => s.Doctor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(x =>
                x.ServiceName.ToLower().Contains(s) ||
                x.Department.DepartmentName.ToLower().Contains(s) ||
                x.Doctor.DoctorName.ToLower().Contains(s));
        }

        var totalCount = await query.CountAsync();

        var pageData = await query
            .OrderBy(x => x.ServiceName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dto = _mapper.Map<IEnumerable<ServiceDto>>(pageData);

        return new { totalCount, data = dto };
    }

}