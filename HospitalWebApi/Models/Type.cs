using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class Type
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string TypeName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<Type> InverseParent { get; set; } = new List<Type>();

    public virtual ICollection<Outsource> Outsources { get; set; } = new List<Outsource>();

    public virtual Type? Parent { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
