using Volo.Abp.Modularity;

namespace SnAbp.Problem
{
    [DependsOn(
        typeof(ProblemApplicationModule),
        typeof(ProblemDomainTestModule)
        )]
    public class ProblemApplicationTestModule : AbpModule
    {

    }
}
