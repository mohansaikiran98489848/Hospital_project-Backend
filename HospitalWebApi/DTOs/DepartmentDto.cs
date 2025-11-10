namespace HospitalWebApi.DTOs
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string? Description { get; set; }
        public int TypeId { get; set; }
    }
}
