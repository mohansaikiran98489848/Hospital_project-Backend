using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public int TypeId { get; set; }

    public int DepartmentId { get; set; }

    public int DoctorId { get; set; }

    public string ServiceName { get; set; } = null!;

    public decimal Fee { get; set; }

    public string? Description { get; set; }

    public int? OutsourceId { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual Department Department { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Outsource? Outsource { get; set; }

    public virtual ICollection<ServiceReceipt> ServiceReceipts { get; set; } = new List<ServiceReceipt>();

    public virtual Type Type { get; set; } = null!;
}
