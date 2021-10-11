using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Construction.EntityFrameworkCore
{
    public class ConstructionHttpApiHostMigrationsDbContext : AbpDbContext<ConstructionHttpApiHostMigrationsDbContext>
    {
        public ConstructionHttpApiHostMigrationsDbContext(DbContextOptions<ConstructionHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureConstruction();
        }
    }
}
