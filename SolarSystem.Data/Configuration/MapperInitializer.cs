using AutoMapper;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;

namespace SolarSystem.Data.Configuration
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<Region, RegionDetailDTO>().ReverseMap();
            CreateMap<Region, CreateRegionDTO>().ReverseMap();

            CreateMap<Component, ComponentDTO>().ReverseMap();
            CreateMap<Component, ComponentDetailDTO>().ReverseMap();
            CreateMap<Component, CreateComponentDTO>().ReverseMap();

            CreateMap<Body, BodyDTO>().ReverseMap();
            CreateMap<Body, BodyDetailDTO>().ReverseMap();
            CreateMap<Body, ManageBodyDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, RegisterDTO>().ReverseMap();
        }
    }
}
