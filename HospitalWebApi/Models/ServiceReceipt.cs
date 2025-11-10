using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class ServiceReceipt
{
    public int ServiceReceiptId { get; set; }

    public int BillHeaderId { get; set; }

    public int ServiceId { get; set; }

    public int? Quantity { get; set; }

    public decimal Amount { get; set; }

    public virtual BillHeader BillHeader { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
