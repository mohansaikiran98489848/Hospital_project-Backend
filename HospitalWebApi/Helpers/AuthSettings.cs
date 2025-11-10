namespace HospitalWebApi.Helpers
{
    public class AuthSettings
    {
        public string SecretKey { get; set; } = null!;
        public int TokenExpiryMinutes { get; set; }
    }
}
