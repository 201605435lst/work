using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Resource.EntityFrameworkCore
{
    public class ResourceHttpApiHostMigrationsDbContext : AbpDbContext<ResourceHttpApiHostMigrationsDbContext>
    {
        public ResourceHttpApiHostMigrationsDbContext(DbContextOptions<ResourceHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureResource();
        }
    }
}
