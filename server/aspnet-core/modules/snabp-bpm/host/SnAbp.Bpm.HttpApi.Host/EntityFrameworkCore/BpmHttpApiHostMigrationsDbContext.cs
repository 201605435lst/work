using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Bpm.EntityFrameworkCore
{
    public class BpmHttpApiHostMigrationsDbContext : AbpDbContext<BpmHttpApiHostMigrationsDbContext>
    {
        public BpmHttpApiHostMigrationsDbContext(DbContextOptions<BpmHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureBpm();
        }
    }
}
