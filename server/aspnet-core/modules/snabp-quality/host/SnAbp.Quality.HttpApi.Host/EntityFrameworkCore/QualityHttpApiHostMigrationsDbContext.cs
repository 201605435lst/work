using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Quality.EntityFrameworkCore
{
    public class QualityHttpApiHostMigrationsDbContext : AbpDbContext<QualityHttpApiHostMigrationsDbContext>
    {
        public QualityHttpApiHostMigrationsDbContext(DbContextOptions<QualityHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureQuality();
        }
    }
}
