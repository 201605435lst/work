using SnAbp.Account;
using SnAbp.FeatureManagement;
using SnAbp.Identity;
using SnAbp.PermissionManagement;
using SnAbp.TenantManagement;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;


namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainSharedModule),
        typeof(SnAbpAccountApplicationContractsModule),
        typeof(SnAbpFeatureManagementApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(SnAbpPermissionManagementApplicationContractsModule),
        typeof(SnAbpTenantManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class MyProjectNameApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            MyProjectNameDtoExtensions.Configure();
        }
    }
}
