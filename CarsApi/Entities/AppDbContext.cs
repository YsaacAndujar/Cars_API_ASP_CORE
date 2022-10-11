using Microsoft.EntityFrameworkCore;

namespace CarsApi.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarsDesigners>()
                .HasKey(x => new { x.CarId, x.DesignerId });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Designer> Designers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarsDesigners> CarsDesigners { get; set; }

    }
}
