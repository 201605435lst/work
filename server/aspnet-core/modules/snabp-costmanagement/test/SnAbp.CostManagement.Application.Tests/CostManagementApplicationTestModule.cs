using Volo.Abp.Modularity;

namespace SnAbp.CostManagement
{
    [DependsOn(
        typeof(CostManagementApplicationModule),
        typeof(CostManagementDomainTestModule)
        )]
    public class CostManagementApplicationTestModule : AbpModule
    {

    }
}
