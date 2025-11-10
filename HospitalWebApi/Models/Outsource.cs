using System;
using System.Collections.Generic;

namespace HospitalWebApi.Models;

public partial class Outsource
{
    public int OutsourceId { get; set; }

    public int TypeId { get; set; }

    public string OutsourceName { get; set; } = null!;

    public string? ContactPerson { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateTime? ContractStartDate { get; set; }

    public DateTime? ContractEndDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual Type Type { get; set; } = null!;
}
