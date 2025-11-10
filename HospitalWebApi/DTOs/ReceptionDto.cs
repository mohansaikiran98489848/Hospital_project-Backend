namespace HospitalWebApi.DTOs
{
    public class ReceptionDto
    {
        public int PatientId { get; set; }           // 0 for new patients
        public string PatientName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int? ServiceId { get; set; }           // e.g., Consultation/Lab
        public decimal? PaidAmount { get; set; }      // optional
        public string? PaymentMode { get; set; }      // Cash, Card, UPI, etc.
    }
}
