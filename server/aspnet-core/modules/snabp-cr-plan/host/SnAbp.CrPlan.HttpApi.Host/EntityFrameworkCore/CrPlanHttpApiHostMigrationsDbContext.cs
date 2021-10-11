using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.CrPlan.EntityFrameworkCore
{
    public class CrPlanHttpApiHostMigrationsDbContext : AbpDbContext<CrPlanHttpApiHostMigrationsDbContext>
    {
        public CrPlanHttpApiHostMigrationsDbContext(DbContextOptions<CrPlanHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureCrPlan();
        }
    }
}
