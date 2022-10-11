namespace CarsApi.Entities
{
    public class CarsDesigners
    {
        public int CarId { get; set; }
        public int DesignerId { get; set; }
        public Car Car { get; set; }
        public Designer Designer { get; set; }
    }
}
