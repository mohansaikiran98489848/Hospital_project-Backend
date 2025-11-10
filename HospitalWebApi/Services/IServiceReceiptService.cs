using AutoMapper;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;

public interface IServiceReceiptService
{
    Task<IEnumerable<ServiceReceiptDto>> GetAllAsync();
    Task<ServiceReceiptDto?> GetByIdAsync(int id);
    Task<ServiceReceiptDto> CreateAsync(ServiceReceiptDto dto);
    Task<bool> UpdateAsync(int id, ServiceReceiptDto dto);
    Task<bool> DeleteAsync(int id);
}



public class ServiceReceiptService : IServiceReceiptService
{
    private readonly HospitalContext _context;
    private readonly IMapper _mapper;

    public ServiceReceiptService(HospitalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceReceiptDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<ServiceReceiptDto>>(await _context.ServiceReceipts.ToListAsync());

    public async Task<ServiceReceiptDto?> GetByIdAsync(int id)
    {
        var entity = await _context.ServiceReceipts.FindAsync(id);
        return entity == null ? null : _mapper.Map<ServiceReceiptDto>(entity);
    }

    public async Task<ServiceReceiptDto> CreateAsync(ServiceReceiptDto dto)
    {
        var entity = _mapper.Map<ServiceReceipt>(dto);
        _context.ServiceReceipts.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<ServiceReceiptDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, ServiceReceiptDto dto)
    {
        var entity = await _context.ServiceReceipts.FindAsync(id);
        if (entity == null) return false;
        _mapper.Map(dto, entity);
        _context.ServiceReceipts.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.ServiceReceipts.FindAsync(id);
        if (entity == null) return false;
        _context.ServiceReceipts.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
