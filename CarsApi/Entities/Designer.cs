using System.ComponentModel.DataAnnotations;

namespace CarsApi.Entities
{
    public class Designer
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Photo { get; set; }
        public List<CarsDesigners> CarsDesigners { get; set; }
    }
}
