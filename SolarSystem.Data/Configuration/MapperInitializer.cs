using AutoMapper;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem.Data.Configuration
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<Region, RegionDetailDTO>().ReverseMap();
            CreateMap<Region, ManageRegionDTO>().ReverseMap();

            CreateMap<Component, ComponentDTO>().ReverseMap();
            CreateMap<Component, ComponentDetailDTO>().ReverseMap();
            CreateMap<Component, ManageComponentDTO>().ReverseMap();

            CreateMap<Body, BodyDTO>().ReverseMap();
            CreateMap<Body, BodyDetailDTO>().ReverseMap();
            CreateMap<Body, ManageBodyDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
