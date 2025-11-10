namespace HospitalWebApi.DTOs
{
    public class UserDto
    {
        public int? UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? Password { get; set; } 
        public int RoleId { get; set; }
        public string? RoleName { get; set; } 
    }
}
