using CarsApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace CarsApi.DTOs
{
    public class CarPostDTO
    {
        [Required]
        [StringLength(50)]
        public string Model { get; set; }
        [FileSizeValidation(max: 4)]
        [FileTypeValidation(fileEnumType: FileEnumType.Image)]
        public IFormFile Photo { get; set; }
    }
}
