using Microsoft.EntityFrameworkCore;
using SnAbp.Problem.Entities;
//using SnAbp.Problem.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Problem.EntityFrameworkCore
{
    [ConnectionStringName(ProblemDbProperties.ConnectionStringName)]
    public class ProblemDbContext : AbpDbContext<ProblemDbContext>, IProblemDbContext
    {
        public DbSet<Entities.Problem> Problem { get; set; }
        public DbSet<ProblemCategory> ProblemCategory { get; set; }
        public DbSet<ProblemRltProblemCategory> ProblemRltProblemCategory { get; set; }

        public ProblemDbContext(DbContextOptions<ProblemDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureProblem();
        }
    }
}