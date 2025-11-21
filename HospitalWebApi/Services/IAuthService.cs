using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using HospitalWebApi.Helpers;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginDto dto);
}

public class AuthService : IAuthService
{
    private readonly HospitalContext _context;
    private readonly AuthSettings _authSettings;

    public AuthService(HospitalContext context, IOptions<AuthSettings> settings)
    {
        _context = context;
        _authSettings = settings.Value;
    }
    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }

    /* public async Task<string?> LoginAsync(LoginDto dto)
     {
         if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
             return null;

         var user = await _context.Users
             .Include(u => u.Role)
             .FirstOrDefaultAsync(u => u.Username == dto.Username);

         if (user == null)
             return null;

         var hashedInput = HashPassword(dto.Password);
         if (hashedInput != user.PasswordHash)
             return null;


         var tokenHandler = new JwtSecurityTokenHandler();
         var key = Encoding.ASCII.GetBytes(_authSettings.SecretKey);

         var tokenDescriptor = new SecurityTokenDescriptor
         {
             Subject = new ClaimsIdentity(new[]
             {
                 new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                 new Claim(ClaimTypes.Name, user.Username),
                 new Claim(ClaimTypes.Role, user.Role.RoleName)
             }),
             Expires = DateTime.UtcNow.AddHours(1),
             SigningCredentials = new SigningCredentials(
                 new SymmetricSecurityKey(key),
                 SecurityAlgorithms.HmacSha256Signature)
         };

         var token = tokenHandler.CreateToken(tokenDescriptor);
         return tokenHandler.WriteToken(token);
    }
    */
   public async Task<string?> LoginAsync(LoginDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
        return null;

    var user = await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.Username == dto.Username);

    if (user == null)
        return null;

    var hashedInput = HashPassword(dto.Password);
    if (hashedInput != user.PasswordHash)
        return null;

    // ==============================================
    // 🔥 FIX: Find doctor correctly (NO UserId needed)
    // ==============================================
    string loginUserName = user.Username.Trim();
    string? doctorName = null;

    if (user.Role.RoleName == "Doctor")
    {
        doctorName = await _context.Doctors
            .Where(d =>
                d.Email == loginUserName ||
                d.DoctorName.Replace(" ", "").ToLower() ==
                loginUserName.Replace(" ", "").ToLower()
            )
            .Select(d => d.DoctorName)
            .FirstOrDefaultAsync();
    }

    var nameToUse = doctorName ?? user.Username;

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_authSettings.SecretKey);

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Role, user.Role.RoleName),
        new Claim(ClaimTypes.Name, nameToUse)
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}



}
