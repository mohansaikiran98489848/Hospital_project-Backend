using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string PatientName { get; set; } = null!;

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? RegisteredDate { get; set; }

    public virtual ICollection<BillHeader> BillHeaders { get; set; } = new List<BillHeader>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
}
