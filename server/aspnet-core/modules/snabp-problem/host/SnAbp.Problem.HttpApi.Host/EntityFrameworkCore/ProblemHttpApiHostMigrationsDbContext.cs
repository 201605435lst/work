using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Problem.EntityFrameworkCore
{
    public class ProblemHttpApiHostMigrationsDbContext : AbpDbContext<ProblemHttpApiHostMigrationsDbContext>
    {
        public ProblemHttpApiHostMigrationsDbContext(DbContextOptions<ProblemHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureProblem();
        }
    }
}
