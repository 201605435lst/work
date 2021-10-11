using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Cms.EntityFrameworkCore
{
    public class CmsHttpApiHostMigrationsDbContext : AbpDbContext<CmsHttpApiHostMigrationsDbContext>
    {
        public CmsHttpApiHostMigrationsDbContext(DbContextOptions<CmsHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureCms();
        }
    }
}
