namespace HospitalWebApi.DTOs
{
    public class ReceptionResponseDto
    {
        public int BillHeaderId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
