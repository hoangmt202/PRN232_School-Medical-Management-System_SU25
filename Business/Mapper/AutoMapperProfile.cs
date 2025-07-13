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
        }
    }
}
