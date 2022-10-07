using Microsoft.EntityFrameworkCore;

namespace CarsApi.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
    }
}
