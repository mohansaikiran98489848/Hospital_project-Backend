namespace HospitalWebApi.DTOs
{
    public class OutsourceDto
    {
        public int OutsourceId { get; set; }
        public string OutsourceName { get; set; } = null!;
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int TypeId { get; set; }
    }
}
