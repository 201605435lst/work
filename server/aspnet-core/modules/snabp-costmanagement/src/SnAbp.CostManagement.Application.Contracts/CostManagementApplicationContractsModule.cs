using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.CostManagement
{
    [DependsOn(
        typeof(CostManagementDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class CostManagementApplicationContractsModule : AbpModule
    {

    }
}
