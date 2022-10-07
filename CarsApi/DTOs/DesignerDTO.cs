using System.ComponentModel.DataAnnotations;

namespace CarsApi.DTOs
{
    public class DesignerDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Photo { get; set; }
    }
}
