using System;

namespace HospitalWebApi.DTOs
{
    public class VisitDto
    {
        public int VisitId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ConsultationId { get; set; }
        public string VisitType { get; set; } = "Normal";
        public int? ReferredDoctorId { get; set; }
        public int VisitNumber { get; set; }
        public string? Notes { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
