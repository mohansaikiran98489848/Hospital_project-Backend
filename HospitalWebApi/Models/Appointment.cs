using System;

namespace HospitalWebApi.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateTime ScheduledAt { get; set; }

        // Pending | Completed | Cancelled
        public string Status { get; set; } = "Pending";

        // Fresh | FollowUp | Forward | Referred
        public string VisitType { get; set; } = "Fresh";

        public string? Notes { get; set; }

        public int? ConsultationId { get; set; }

        // navigation
        public virtual Patient? Patient { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public virtual Consultation? Consultation { get; set; }
    }
}
