using System.ComponentModel.DataAnnotations;

namespace CarsApi.DTOs
{
    public class CarDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Model { get; set; }
        public string Photo { get; set; }
    }
}
