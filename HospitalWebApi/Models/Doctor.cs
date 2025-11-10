using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public int TypeId { get; set; }

    // ✅ NEW — Link doctor to department
    public int DepartmentId { get; set; }

    public string DoctorName { get; set; } = null!;

    public string? Qualification { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual Type Type { get; set; } = null!;
}
