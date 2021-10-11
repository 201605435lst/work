using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Technology.EntityFrameworkCore
{
    public class TechnologyHttpApiHostMigrationsDbContext : AbpDbContext<TechnologyHttpApiHostMigrationsDbContext>
    {
        public TechnologyHttpApiHostMigrationsDbContext(DbContextOptions<TechnologyHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureTechnology();
        }
    }
}
