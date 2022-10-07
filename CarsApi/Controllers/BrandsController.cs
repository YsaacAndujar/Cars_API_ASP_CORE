using AutoMapper;
using CarsApi.DTOs;
using CarsApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarsApi.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandsController: ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public BrandsController(AppDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<BrandDTO>>> Get()
        {
            var entities = await context.Brands.ToListAsync();
            var data = mapper.Map<List<BrandDTO>>(entities);
            return data;
        }

        [HttpGet("{id:int}", Name = "getBrand")]
        public async Task<ActionResult<BrandDTO>> Get(int id)
        {
            var entity = await context.Brands.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            var data = mapper.Map<BrandDTO>(entity);
            return data;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BrandPostDTO brandPostDTO)
        {
            var entity = mapper.Map<Brand>(brandPostDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            var brandDTO = mapper.Map<BrandDTO>(entity);
            return new CreatedAtRouteResult("getBrand", new {id = entity.Id}, brandDTO);
        }
    }
}
