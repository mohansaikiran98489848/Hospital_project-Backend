using AutoMapper;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;

public interface IBillReceiptService
{
    Task<IEnumerable<BillReceiptDto>> GetAllAsync(int? billHeaderId = null);
    Task<BillReceiptDto?> GetByIdAsync(int id);
    Task<BillReceiptDto> CreateAsync(BillReceiptDto dto);
    Task<bool> UpdateAsync(int id, BillReceiptDto dto);
    Task<bool> DeleteAsync(int id);
}

public class BillReceiptService : IBillReceiptService
{
    private readonly HospitalContext _context;
    private readonly IMapper _mapper;

    public BillReceiptService(HospitalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // ✅ Now handles optional filtering by billHeaderId
    public async Task<IEnumerable<BillReceiptDto>> GetAllAsync(int? billHeaderId = null)
    {
        var query = _context.BillReceipts.AsQueryable();

        if (billHeaderId.HasValue)
            query = query.Where(b => b.BillHeaderId == billHeaderId.Value);

        var list = await query.ToListAsync();
        return _mapper.Map<IEnumerable<BillReceiptDto>>(list);
    }

    public async Task<BillReceiptDto?> GetByIdAsync(int id)
    {
        var entity = await _context.BillReceipts.FindAsync(id);
        return entity == null ? null : _mapper.Map<BillReceiptDto>(entity);
    }

    public async Task<BillReceiptDto> CreateAsync(BillReceiptDto dto)
    {
        var entity = _mapper.Map<BillReceipt>(dto);
        _context.BillReceipts.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<BillReceiptDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, BillReceiptDto dto)
    {
        var entity = await _context.BillReceipts.FindAsync(id);
        if (entity == null) return false;

        _mapper.Map(dto, entity);
        _context.BillReceipts.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.BillReceipts.FindAsync(id);
        if (entity == null) return false;

        _context.BillReceipts.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
