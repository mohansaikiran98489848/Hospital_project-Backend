namespace HospitalWebApi.DTOs
{
    public class BillReceiptDto
    {
        public int BillReceiptId { get; set; }
        public int BillHeaderId { get; set; }
        public decimal PaidAmount { get; set; }
        public string? PaymentMode { get; set; }

        public DateTime? PaymentDate { get; set; }

    }
}
