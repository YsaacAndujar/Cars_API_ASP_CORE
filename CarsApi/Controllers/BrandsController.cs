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

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] BrandPostDTO brandPostDTO)
        {
            var exists = await context.Brands.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            var entity = mapper.Map<Brand>(brandPostDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Brands.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            context.Remove(new Brand() { Id=id});
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
