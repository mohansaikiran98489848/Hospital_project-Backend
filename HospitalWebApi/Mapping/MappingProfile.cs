using HospitalWebApi.DTOs;
using HospitalWebApi.Models;
using AutoMapper;
namespace HospitalWebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Doctor, DoctorDto>()
      .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
      .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.TypeId))          // ✅ added
      .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId)) // ✅ added
      .ReverseMap();


            CreateMap<Department, DepartmentDto>().ReverseMap();

            CreateMap<Service, ServiceDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.DoctorName))
            .ForMember(dest => dest.OutsourceName, opt => opt.MapFrom(src => src.Outsource != null ? src.Outsource.OutsourceName : null))
            .ForMember(dest => dest.OutsourceId, opt => opt.MapFrom(src => src.OutsourceId));

            CreateMap<ServiceDto, Service>()
    .ForMember(dest => dest.ServiceId, opt => opt.Ignore()); // prevent key overwrite

            CreateMap<Consultation, ConsultationDto>()
     .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.PatientName))
     .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.DoctorName))
     .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName))
     .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Service.Department.DepartmentName)); // ✅ ADDED


            CreateMap<ConsultationDto, Consultation>();

            CreateMap<BillHeader, BillHeaderDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.PatientName));
            CreateMap<BillHeaderDto, BillHeader>();

            CreateMap<Models.Type, TypeDto>()
      .ForMember(d => d.ParentName, o => o.MapFrom(s => s.Parent != null ? s.Parent.TypeName : null))
      .ReverseMap();


            CreateMap<User, UserDto>()
        .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());


            CreateMap<ServiceReceipt, ServiceReceiptDto>().ReverseMap();
            CreateMap<BillReceipt, BillReceiptDto>().ReverseMap();
            CreateMap<Outsource, OutsourceDto>().ReverseMap();

            CreateMap<Appointment, AppointmentDto>()
     .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.PatientName))
     .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.DoctorName))
     .ReverseMap();


            CreateMap<Visit, VisitDto>().ReverseMap();

        }
    }
}
