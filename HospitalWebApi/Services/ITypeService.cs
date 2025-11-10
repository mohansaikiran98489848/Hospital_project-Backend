using AutoMapper;
using AutoMapper.QueryableExtensions;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalWebApi.Services
{
    public interface ITypeService
    {
        Task<IEnumerable<TypeDto>> GetAllAsync();
        Task<PagedResult<TypeDto>> GetPagedAsync(int page, int pageSize, string? search);
        Task<TypeDto?> GetByIdAsync(int id);
        Task<TypeDto> CreateAsync(TypeDto dto);
        Task<TypeDto?> UpdateAsync(int id, TypeDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public class TypeService : ITypeService
    {
        private readonly HospitalContext _context;
        private readonly IMapper _mapper;

        public TypeService(HospitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TypeDto>> GetAllAsync()
        {
            // full list (used for Parent dropdown)
            return await _context.Types
                .AsNoTracking()
                .Include(t => t.Parent)
                .ProjectTo<TypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PagedResult<TypeDto>> GetPagedAsync(int page, int pageSize, string? search)
        {
            var query = _context.Types
                .AsNoTracking()
                .Include(t => t.Parent)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(t => t.TypeName.ToLower().Contains(term));
            }

            var total = await query.CountAsync();

            var rows = await query
                .OrderBy(t => t.TypeName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<TypeDto>
            {
                TotalCount = total,
                Data = rows
            };
        }

        public async Task<TypeDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Types
                .AsNoTracking()
                .Include(t => t.Parent)
                .FirstOrDefaultAsync(t => t.Id == id);

            return entity == null ? null : _mapper.Map<TypeDto>(entity);
        }

        public async Task<TypeDto> CreateAsync(TypeDto dto)
        {
            var entity = _mapper.Map<Models.Type>(dto);
            entity.Id = 0; // force create
            _context.Types.Add(entity);
            await _context.SaveChangesAsync();

            var saved = await _context.Types.Include(x => x.Parent).FirstAsync(x => x.Id == entity.Id);
            return _mapper.Map<TypeDto>(saved);
        }

        public async Task<TypeDto?> UpdateAsync(int id, TypeDto dto)
        {
            var entity = await _context.Types.FindAsync(id);
            if (entity == null) return null;

            // prevent key overwrite; map other fields
            entity.ParentId = dto.ParentId;
            entity.TypeName = dto.TypeName;
            entity.Description = dto.Description;

            await _context.SaveChangesAsync();

            var saved = await _context.Types.Include(x => x.Parent).FirstAsync(x => x.Id == id);
            return _mapper.Map<TypeDto>(saved);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Types
                .Include(t => t.InverseParent) // make sure we don’t break FK if children exist
                .FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null) return false;

            if (entity.InverseParent.Any())
            {
                // Business rule: avoid deleting when children exist.
                // You can cascade in DB if you prefer.
                throw new InvalidOperationException("Cannot delete a Type that has child Types. Reassign or delete children first.");
            }

            _context.Types.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
