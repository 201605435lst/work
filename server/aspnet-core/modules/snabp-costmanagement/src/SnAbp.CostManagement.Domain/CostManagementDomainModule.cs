using Volo.Abp.Modularity;

namespace SnAbp.CostManagement
{
    [DependsOn(
        typeof(CostManagementDomainSharedModule)
        )]
    public class CostManagementDomainModule : AbpModule
    {

    }
}
