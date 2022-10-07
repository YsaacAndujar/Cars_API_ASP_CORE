using CarsApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace CarsApi.DTOs
{
    public class DesignerPostDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [FileSizeValidation(max: 4)]
        [FileTypeValidation(fileEnumType:FileEnumType.Image)]
        public IFormFile Photo { get; set; }
    }
}
