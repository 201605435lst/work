using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Common.EntityFrameworkCore
{
    public class CommonHttpApiHostMigrationsDbContext : AbpDbContext<CommonHttpApiHostMigrationsDbContext>
    {
        public CommonHttpApiHostMigrationsDbContext(DbContextOptions<CommonHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureCommon();
        }
    }
}
