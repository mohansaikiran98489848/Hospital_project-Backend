using AutoMapper;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;

public interface IBillHeaderService
{
    Task<IEnumerable<BillHeaderDto>> GetAllAsync();
    Task<BillHeaderDto?> GetByIdAsync(int id);
    Task<BillHeaderDto> CreateAsync(BillHeaderDto dto);
    Task<bool> UpdateAsync(int id, BillHeaderDto dto);
    Task<bool> DeleteAsync(int id);
}



public class BillHeaderService : IBillHeaderService
{
    private readonly HospitalContext _context;
    private readonly IMapper _mapper;

    public BillHeaderService(HospitalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BillHeaderDto>> GetAllAsync() =>
     _mapper.Map<IEnumerable<BillHeaderDto>>(await _context.BillHeaders
         .Include(b => b.Patient)   
         .ToListAsync());

    public async Task<BillHeaderDto?> GetByIdAsync(int id)
    {
        var entity = await _context.BillHeaders
            .Include(b => b.Patient)  
            .FirstOrDefaultAsync(b => b.BillHeaderId == id);
        return entity == null ? null : _mapper.Map<BillHeaderDto>(entity);
    }

    public async Task<BillHeaderDto> CreateAsync(BillHeaderDto dto)
    {
        var entity = _mapper.Map<BillHeader>(dto);
        _context.BillHeaders.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<BillHeaderDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, BillHeaderDto dto)
    {
        var entity = await _context.BillHeaders.FindAsync(id);
        if (entity == null) return false;
        _mapper.Map(dto, entity);
        _context.BillHeaders.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.BillHeaders.FindAsync(id);
        if (entity == null) return false;
        _context.BillHeaders.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}