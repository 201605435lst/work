using Volo.Abp.Modularity;

namespace SnAbp.CrPlan
{
    [DependsOn(
        typeof(CrPlanDomainSharedModule)
        )]
    public class CrPlanDomainModule : AbpModule
    {

    }
}
