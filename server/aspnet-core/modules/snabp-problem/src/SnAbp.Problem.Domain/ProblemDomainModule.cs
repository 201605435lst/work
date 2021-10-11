using Volo.Abp.Modularity;

namespace SnAbp.Problem
{
    [DependsOn(
        typeof(ProblemDomainSharedModule)
        )]
    public class ProblemDomainModule : AbpModule
    {

    }
}
