using AutoMapper;
using CarsApi.DTOs;
using CarsApi.Entities;
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

        public DesignersController(AppDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            this.context = context;
            this.mapper = mapper;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<List<DesignerDTO>>> Get()
        {
            var entities = await context.Designers.ToListAsync();
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
            entity.Photo = saveImg(designerPostDTO.Photo);
            context.Add(entity);
            await context.SaveChangesAsync();
            var designerDTO = mapper.Map<DesignerDTO>(entity);
            return new CreatedAtRouteResult("getDesigner", new { id = entity.Id }, designerDTO);
        }

        string saveImg(IFormFile formFile)
        {
            if(formFile == null)
            {
                return null;
            }
            if (formFile.Length <= 0)
            {
                return null;
            }
            var fileExtension = formFile.ContentType.Split("/").LastOrDefault();
            if (fileExtension == null)
            {
                return null;
            }
            var baseRoute = $"{environment.WebRootPath}\\images\\";
            if (!Directory.Exists(baseRoute))
            {
                Directory.CreateDirectory(baseRoute);
            }
            var fileRoute = DateTime.Now.ToString("yyMMddHHmmssff") + "." + fileExtension;
            var fileAbsoluteRoute = baseRoute + fileRoute;
            using (FileStream fileStream = System.IO.File.Create(fileAbsoluteRoute))
            {
                formFile.CopyToAsync(fileStream);
                fileStream.Flush();
                return "/media/"+fileRoute;
            }
        }
        void deleteImg(string fileRoute)
        {
            var baseRoute = $"{environment.WebRootPath}\\images\\";
            fileRoute = fileRoute.Remove(0, 6);
            var fileAbsoluteRoute = baseRoute + fileRoute;
            System.IO.File.Delete(fileAbsoluteRoute);
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
                deleteImg(entity.Photo);
                entity.Photo = saveImg(designerPostDTO.Photo);
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
                deleteImg(entity.Photo);
            }
            context.Remove(entity);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
