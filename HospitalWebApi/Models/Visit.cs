using System;

namespace HospitalWebApi.Models
{
    public class Visit
    {
        public int VisitId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ConsultationId { get; set; }

        // Normal | FollowUp | Forward | Referred
        public string VisitType { get; set; } = "Normal";

        public int? ReferredDoctorId { get; set; }

        public int VisitNumber { get; set; }

        public string? Notes { get; set; }

        public DateTime VisitDate { get; set; } = DateTime.UtcNow;

        // navigation
        public virtual Patient? Patient { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public virtual Consultation? Consultation { get; set; }
        public virtual Doctor? ReferredDoctor { get; set; }
    }
}
