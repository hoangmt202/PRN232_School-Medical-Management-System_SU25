using AutoMapper;
using BusinessLogic.DTOs;
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
            CreateMap<ParentRequestDTO, Parent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
