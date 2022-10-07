using AutoMapper;
using CarsApi.DTOs;
using CarsApi.Entities;

namespace CarsApi.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<BrandPostDTO, Brand>();
        }
    }
}
