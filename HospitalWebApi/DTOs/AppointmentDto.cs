using System;

namespace HospitalWebApi.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Status { get; set; } = "Pending";
        public string VisitType { get; set; } = "Fresh";
        public string? Notes { get; set; }
        public int? ConsultationId { get; set; }

        // optional navigation summaries (not required)
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
    }
}
