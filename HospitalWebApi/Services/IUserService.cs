using AutoMapper;
using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<UserDto?> CreateAsync(UserDto dto);  
    Task<bool> UpdateAsync(int id, UserDto dto);
    Task<bool> DeleteAsync(int id);
}
public class UserService : IUserService
{
    private readonly HospitalContext _context;
    private readonly IMapper _mapper;

    public UserService(HospitalContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _context.Users.Include(u => u.Role).ToListAsync();
        var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

        foreach (var dto in dtos)
            dto.Password = "*****"; 

        return dtos;
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users.Include(u => u.Role)
                                       .FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null) return null;

        var dto = _mapper.Map<UserDto>(user);
        dto.Password = "*****";
        return dto;
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        var user = await _context.Users.Include(u => u.Role)
                                       .FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return null;

        var dto = _mapper.Map<UserDto>(user);
        dto.Password = "******";
        return dto;
    }

    public async Task<UserDto?> CreateAsync(UserDto dto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (existingUser != null)
            return null;

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = HashPassword(dto.Password ?? string.Empty);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var createdDto = _mapper.Map<UserDto>(user);
        createdDto.Password = null;
        return createdDto;
    }

    public async Task<bool> UpdateAsync(int id, UserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        var duplicate = await _context.Users
            .AnyAsync(u => u.Username == dto.Username && u.UserId != id);
        if (duplicate)
            return false; // username conflict

        user.Username = dto.Username;
        user.RoleId = dto.RoleId;

        if (!string.IsNullOrEmpty(dto.Password))
            user.PasswordHash = HashPassword(dto.Password);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}

