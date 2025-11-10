// DTOs/ServiceDto.cs
namespace HospitalWebApi.DTOs;
public class ServiceDto
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = null!;
    public decimal Fee { get; set; }
    public string? Description { get; set; }

    public int TypeId { get; set; }
    public string? TypeName { get; set; }  

    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; } 

    public int DoctorId { get; set; }
    public string? DoctorName { get; set; }  

    public int? OutsourceId { get; set; }
    public string? OutsourceName { get; set; } 
}
