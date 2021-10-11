using SnAbp.Problem.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Problem
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(ProblemEntityFrameworkCoreTestModule)
        )]
    public class ProblemDomainTestModule : AbpModule
    {
        
    }
}
