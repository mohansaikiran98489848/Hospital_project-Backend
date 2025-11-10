namespace HospitalWebApi.DTOs
{
    public class ServiceReceiptDto
    {
        public int ServiceReceiptId { get; set; }
        public int BillHeaderId { get; set; }
        public int ServiceId { get; set; }
        public int? Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
