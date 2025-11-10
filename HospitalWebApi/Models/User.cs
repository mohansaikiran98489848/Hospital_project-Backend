namespace HospitalWebApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;
    }
}
