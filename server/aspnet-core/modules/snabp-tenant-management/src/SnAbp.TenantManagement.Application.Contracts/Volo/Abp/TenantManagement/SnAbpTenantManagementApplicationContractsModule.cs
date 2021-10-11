using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace SnAbp.TenantManagement
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(SnAbpTenantManagementDomainSharedModule))]
    public class SnAbpTenantManagementApplicationContractsModule : AbpModule
    {

    }
}