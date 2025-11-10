namespace HospitalWebApi.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = null!;
        public string? Qualification { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int TypeId { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
    }

}
