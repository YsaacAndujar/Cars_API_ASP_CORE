using System.ComponentModel.DataAnnotations;

namespace CarsApi.DTOs
{
    public class BrandPostDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
