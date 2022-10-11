using AutoMapper;
using CarsApi.DTOs;
using CarsApi.Entities;
using CarsApi.Helpers;
using CarsApi.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarsApi.Controllers
{
    [Route("api/Cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly ILocalFileSaver localFileSaver;

        public CarsController(AppDbContext context, IMapper mapper, ILocalFileSaver localFileSaver)
        {
            this.context = context;
            this.mapper = mapper;
            this.localFileSaver = localFileSaver;
        }

        [HttpGet]
        public async Task<ActionResult<List<CarDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Cars.AsQueryable();
            await HttpContext.InsertParametersPagination(queryable, paginationDTO.EntitiesPerPage);
            var entities = await context.Cars.Paginate(paginationDTO).ToListAsync();
            var data = mapper.Map<List<CarDTO>>(entities);
            return data;
        }

        [HttpGet("{id}", Name = "getCar")]
        public async Task<ActionResult<CarDTO>> Get(int id)
        {
            var entities = await context.Cars
                .Include(x=>x.CarsDesigners)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entities == null)
            {
                return NotFound();
            }
            var data = mapper.Map<CarDTO>(entities);
            return data;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CarPostDTO carPostDTO)
        {
            var entity = mapper.Map<Car>(carPostDTO);
            entity.Photo = await localFileSaver.SaveAsync(carPostDTO.Photo, Validations.FileEnumType.Image);
            context.Add(entity);
            await context.SaveChangesAsync();
            var carDTO = mapper.Map<CarDTO>(entity);
            return new CreatedAtRouteResult("getCar", new { id = entity.Id }, carDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] CarPostDTO carPostDTO)
        {
            var exists = await context.Cars.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            var entity = mapper.Map<Car>(carPostDTO);
            if (entity.Photo != null)
            {
                await localFileSaver.RemoveAsync(entity.Photo, Validations.FileEnumType.Image);
                entity.Photo = await localFileSaver.SaveAsync(carPostDTO.Photo, Validations.FileEnumType.Image);
            }
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await context.Cars.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            if (entity.Photo != null)
            {
                await localFileSaver.RemoveAsync(entity.Photo, Validations.FileEnumType.Image);
            }
            context.Remove(entity);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
