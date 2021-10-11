using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SnAbp.Oa.EntityFrameworkCore
{
    public class OaHttpApiHostMigrationsDbContext : AbpDbContext<OaHttpApiHostMigrationsDbContext>
    {
        public OaHttpApiHostMigrationsDbContext(DbContextOptions<OaHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureOa();
        }
    }
}
