using AutoMapper;
using AutoMapper.QueryableExtensions;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalWebApi.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task<object> GetPagedAsync(int page, int pageSize, string? search);
        Task<DepartmentDto> CreateAsync(DepartmentDto dto);
        Task<bool> UpdateAsync(int id, DepartmentDto dto);
        Task<(bool ok, string? blocked)> DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string name, int? id = null);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly HospitalContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(HospitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync() =>
            await _context.Departments
                .AsNoTracking()
                .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Departments.AsNoTracking()
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
            return entity == null ? null : _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<object> GetPagedAsync(int page, int pageSize, string? search)
        {
            var q = _context.Departments.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(d => d.DepartmentName.ToLower().Contains(s) ||
                                 (d.Description != null && d.Description.ToLower().Contains(s)));
            }

            var totalCount = await q.CountAsync();
            var data = await q.OrderBy(d => d.DepartmentName)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                              .ToListAsync();

            return new { totalCount, data };
        }

        public async Task<DepartmentDto> CreateAsync(DepartmentDto dto)
        {
            var entity = _mapper.Map<Department>(dto);
            _context.Departments.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, DepartmentDto dto)
        {
            var entity = await _context.Departments.FindAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            entity.DepartmentId = id;
            _context.Departments.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool ok, string? blocked)> DeleteAsync(int id)
        {
            var entity = await _context.Departments
                .Include(d => d.Doctors)
                .Include(d => d.Services)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (entity == null) return (false, null);

            if ((entity.Doctors?.Any() ?? false) || (entity.Services?.Any() ?? false))
                return (false, "Cannot delete: Department is in use.");

            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<bool> ExistsByNameAsync(string name, int? id = null)
        {
            return await _context.Departments
                .AnyAsync(d => d.DepartmentName.ToLower() == name.ToLower()
                            && (!id.HasValue || d.DepartmentId != id.Value));
        }
    }
}
