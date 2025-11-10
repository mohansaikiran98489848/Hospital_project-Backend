using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class Consultation
{
    public int ConsultationId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public int ServiceId { get; set; }

    public DateTime? ConsultationDate { get; set; }

    public string? Notes { get; set; }

    public decimal? Fee { get; set; }

    public virtual ICollection<BillHeader> BillHeaders { get; set; } = new List<BillHeader>();

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
