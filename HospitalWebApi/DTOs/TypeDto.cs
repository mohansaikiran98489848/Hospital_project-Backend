namespace HospitalWebApi.DTOs
{
    public class TypeDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string TypeName { get; set; } = null!;
        public string? Description { get; set; }
        public string? ParentName { get; set; }
    }
}
