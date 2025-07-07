using AutoMapper;
using BusinessLogic.DTOs.DrugStorage;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {
            CreateMap<DrugStorage, DrugStorageDto>().ReverseMap();
            CreateMap<DrugStorage, CreateDrugStorageDto>().ReverseMap();
            CreateMap<DrugStorage, UpdateDrugStorageDto>().ReverseMap();
            CreateMap<InventoryAlert, InventoryAlertDto>().ReverseMap();
        }
    }
}
