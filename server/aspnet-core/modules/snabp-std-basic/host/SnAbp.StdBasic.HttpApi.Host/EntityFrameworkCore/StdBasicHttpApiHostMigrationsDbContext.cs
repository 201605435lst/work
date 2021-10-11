using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.StdBasic.EntityFrameworkCore
{
    public class StdBasicHttpApiHostMigrationsDbContext : AbpDbContext<StdBasicHttpApiHostMigrationsDbContext>
    {
        public StdBasicHttpApiHostMigrationsDbContext(DbContextOptions<StdBasicHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureStdBasic();
        }
    }
}
