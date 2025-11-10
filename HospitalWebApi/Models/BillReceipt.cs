using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class BillReceipt
{
    public int BillReceiptId { get; set; }

    public int BillHeaderId { get; set; }

    public decimal PaidAmount { get; set; }

    public string? PaymentMode { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual BillHeader BillHeader { get; set; } = null!;
}
