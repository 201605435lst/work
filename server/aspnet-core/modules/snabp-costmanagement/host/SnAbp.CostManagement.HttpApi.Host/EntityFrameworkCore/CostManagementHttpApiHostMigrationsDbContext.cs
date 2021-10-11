using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.CostManagement.EntityFrameworkCore
{
    public class CostManagementHttpApiHostMigrationsDbContext : AbpDbContext<CostManagementHttpApiHostMigrationsDbContext>
    {
        public CostManagementHttpApiHostMigrationsDbContext(DbContextOptions<CostManagementHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureCostManagement();
        }
    }
}
