using CarsApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace CarsApi.Entities
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Model { get; set; }
        public string Photo { get; set; }
        public List<CarsDesigners> CarsDesigners { get; set; }
    }
}
