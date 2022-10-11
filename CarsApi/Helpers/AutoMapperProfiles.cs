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

            CreateMap<Designer, DesignerDTO>().ReverseMap();
            CreateMap<DesignerPostDTO, Designer>();

            CreateMap<Car, CarDTO>().ReverseMap();
            CreateMap<CarPostDTO, Car>()
                .ForMember(x => x.CarsDesigners, options => options.MapFrom(MapCarsDesigners));
        }
        private List<CarsDesigners> MapCarsDesigners(CarPostDTO carPostDTO, Car cars)
        {
            var result = new List<CarsDesigners>();
            if(carPostDTO == null) { return result; }
            foreach (var id in carPostDTO.CarsDesigners)
            {
                result.Add(new CarsDesigners() { DesignerId = id });
            }
            return result;
        }
    }
}
