using Volo.Abp.Modularity;

namespace SnAbp.CrPlan
{
    [DependsOn(
        typeof(CrPlanApplicationModule),
        typeof(CrPlanDomainTestModule)
        )]
    public class CrPlanApplicationTestModule : AbpModule
    {

    }
}
