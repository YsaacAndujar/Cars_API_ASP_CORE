using CarsApi.Helpers;
using CarsApi.Validations;
using Microsoft.AspNetCore.Mvc;
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
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CarsDesigners { get; set; }
    }
}
