using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Regulation.EntityFrameworkCore
{
    public class RegulationHttpApiHostMigrationsDbContext : AbpDbContext<RegulationHttpApiHostMigrationsDbContext>
    {
        public RegulationHttpApiHostMigrationsDbContext(DbContextOptions<RegulationHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureRegulation();
        }
    }
}
