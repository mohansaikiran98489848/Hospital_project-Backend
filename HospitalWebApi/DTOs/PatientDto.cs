namespace HospitalWebApi.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = null!;
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
