using Microsoft.EntityFrameworkCore;
using SnAbp.Problem.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Problem.EntityFrameworkCore
{
    [ConnectionStringName(ProblemDbProperties.ConnectionStringName)]
    public interface IProblemDbContext : IEfCoreDbContext
    {
        DbSet<Entities.Problem> Problem { get; set; }
        DbSet<ProblemCategory> ProblemCategory { get; set; }
        DbSet<ProblemRltProblemCategory> ProblemRltProblemCategory { get; set; }
    }
}