namespace HospitalWebApi.DTOs
{
    public class BillHeaderDto
    {
        public int BillHeaderId { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public int? ConsultationId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }
    }
}
