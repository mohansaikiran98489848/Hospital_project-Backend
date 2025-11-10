using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class BillHeader
{
    public int BillHeaderId { get; set; }

    public int PatientId { get; set; }

    public int? ConsultationId { get; set; }

    public DateTime? BillDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<BillReceipt> BillReceipts { get; set; } = new List<BillReceipt>();

    public virtual Consultation? Consultation { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<ServiceReceipt> ServiceReceipts { get; set; } = new List<ServiceReceipt>();
}
