using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Material.EntityFrameworkCore
{
    public class MaterialHttpApiHostMigrationsDbContext : AbpDbContext<MaterialHttpApiHostMigrationsDbContext>
    {
        public MaterialHttpApiHostMigrationsDbContext(DbContextOptions<MaterialHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureMaterial();
        }
    }
}
