using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.ConstructionBase.EntityFrameworkCore
{
    public class ConstructionBaseHttpApiHostMigrationsDbContext : AbpDbContext<ConstructionBaseHttpApiHostMigrationsDbContext>
    {
        public ConstructionBaseHttpApiHostMigrationsDbContext(DbContextOptions<ConstructionBaseHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureConstructionBase();
        }
    }
}
