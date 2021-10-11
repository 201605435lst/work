using Volo.Abp.Modularity;

namespace SnAbp.PermissionManagement
{
    [DependsOn(
        typeof(SnAbpPermissionManagementDomainModule), 
        typeof(SnAbpPermissionManagementApplicationContractsModule)
        )]
    public class SnAbpPermissionManagementApplicationModule : AbpModule
    {
        
    }
}
