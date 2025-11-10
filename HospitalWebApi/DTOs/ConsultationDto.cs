namespace HospitalWebApi.DTOs
{
    public class ConsultationDto
    {
        public int ConsultationId { get; set; }

        public int PatientId { get; set; }
        public string? PatientName { get; set; }  

        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }   

        public int ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public string? DepartmentName { get; set; }

        public DateTime? ConsultationDate { get; set; }

        public decimal? Fee { get; set; }
        public string? Notes { get; set; }
    }
}
