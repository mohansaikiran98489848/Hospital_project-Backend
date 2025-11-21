namespace HospitalWebApi.DTOs
{
    public class DoctorAppointmentSummaryDto
    {
        public int DoctorId { get; set; }
        public int Total { get; set; }
        public int Pending { get; set; }
        public int Completed { get; set; }
        public int Fresh { get; set; }
        public int Old { get; set; } // old = not fresh
    }
}
