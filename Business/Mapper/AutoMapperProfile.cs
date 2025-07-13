using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DrugStorage;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Parent, ParentResponseDTO>();
            CreateMap<Parent, ParentDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ParentRequestDTO, Parent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<DrugStorage, DrugStorageDto>().ReverseMap();
            CreateMap<DrugStorage, CreateDrugStorageDto>().ReverseMap();
            CreateMap<DrugStorage, UpdateDrugStorageDto>().ReverseMap();
            CreateMap<InventoryAlert, InventoryAlertDto>().ReverseMap();
            CreateMap<InventoryReport, InventoryReportDto>().ReverseMap();
            CreateMap<VaccinationPlan, VaccinationPlanDto>().ReverseMap();
            CreateMap<Vaccination, VaccinationRecordDto>().ReverseMap();

            CreateMap<Parent, ParentResponseDTO>();
            CreateMap<ParentRequestDTO, Parent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Student
            CreateMap<Student, StudentResponseDTO>();
            CreateMap<StudentRequestDTO, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Student, BusinessLogic.DTOs.Student.StudentDto>();
            CreateMap<BusinessLogic.DTOs.Student.StudentDto, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // SchoolNurse
            CreateMap<SchoolNurse, SchoolNurseResponseDTO>();
            CreateMap<SchoolNurseRequestDTO, SchoolNurse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<SchoolNurse, BusinessLogic.DTOs.SchoolNurse.SchoolNurseDto>();
            CreateMap<BusinessLogic.DTOs.SchoolNurse.SchoolNurseDto, SchoolNurse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<BusinessLogic.DTOs.SchoolNurse.CreateSchoolNurseDto, SchoolNurse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<BusinessLogic.DTOs.SchoolNurse.UpdateSchoolNurseDto, SchoolNurse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Login
            CreateMap<LoginRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<User, LoginResponseDTO>();
        }
    }
}
