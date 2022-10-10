using AutoMapper;
using CarsApi.DTOs;
using CarsApi.Entities;
using CarsApi.Helpers;
using CarsApi.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarsApi.Controllers
{
    [ApiController]
    [Route("api/designers")]
    public class DesignersController: ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment environment;
        private readonly ILocalFileSaver localFileSaver;

        public DesignersController(AppDbContext context, IMapper mapper, IWebHostEnvironment environment,
            ILocalFileSaver localFileSaver)
        {
            this.context = context;
            this.mapper = mapper;
            this.environment = environment;
            this.localFileSaver = localFileSaver;
        }

        [HttpGet]
        public async Task<ActionResult<List<DesignerDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Designers.AsQueryable();
            await HttpContext.InsertParametersPagination(queryable, paginationDTO.EntitiesPerPage);

            var entities = await context.Designers.Paginate(paginationDTO).ToListAsync();
            var data = mapper.Map<List<DesignerDTO>>(entities);
            return data;
        }

        [HttpGet("{id:int}",Name = "getDesigner")]
        public async Task<ActionResult<DesignerDTO>> Get(int id)
        {
            var entities = await context.Designers.FirstOrDefaultAsync(x=>x.Id == id);
            if (entities == null)
            {
                return NotFound();
            }
            var data = mapper.Map<DesignerDTO>(entities);
            return data;
        }

        [HttpPost]
        public  async Task<ActionResult> Post([FromForm] DesignerPostDTO designerPostDTO)
        {
            var entity = mapper.Map<Designer>(designerPostDTO);
            entity.Photo = await localFileSaver.SaveAsync(designerPostDTO.Photo, Validations.FileEnumType.Image);
            context.Add(entity);
            await context.SaveChangesAsync();
            var designerDTO = mapper.Map<DesignerDTO>(entity);
            return new CreatedAtRouteResult("getDesigner", new { id = entity.Id }, designerDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] DesignerPostDTO designerPostDTO)
        {
            var exists = await context.Designers.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            var entity = mapper.Map<Designer>(designerPostDTO);
            if (entity.Photo != null)
            {
                await localFileSaver.RemoveAsync(entity.Photo, Validations.FileEnumType.Image);
                entity.Photo = await localFileSaver.SaveAsync(designerPostDTO.Photo, Validations.FileEnumType.Image);
            }
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await context.Designers.FirstOrDefaultAsync(x => x.Id == id);
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
