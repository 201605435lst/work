using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Safe.EntityFrameworkCore
{
    public class SafeHttpApiHostMigrationsDbContext : AbpDbContext<SafeHttpApiHostMigrationsDbContext>
    {
        public SafeHttpApiHostMigrationsDbContext(DbContextOptions<SafeHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureSafe();
        }
    }
}
