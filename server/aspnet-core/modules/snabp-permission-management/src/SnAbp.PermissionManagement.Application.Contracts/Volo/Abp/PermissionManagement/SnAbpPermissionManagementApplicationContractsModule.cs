using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace SnAbp.PermissionManagement
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(SnAbpPermissionManagementDomainSharedModule))]
    public class SnAbpPermissionManagementApplicationContractsModule : AbpModule
    {
        
    }
}
