using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public int TypeId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string? Description { get; set; }

    // ✅ New: navigation property to doctors
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual Type Type { get; set; } = null!;
}
