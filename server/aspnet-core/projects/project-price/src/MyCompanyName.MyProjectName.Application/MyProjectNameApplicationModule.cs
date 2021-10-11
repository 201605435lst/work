using SnAbp.Account;
using SnAbp.Bpm;
using SnAbp.Cms;
using SnAbp.Common;
using SnAbp.FeatureManagement;
using SnAbp.File;
using SnAbp.Identity;
using SnAbp.PermissionManagement;
using SnAbp.TenantManagement;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(SnAbpAccountApplicationModule),
        typeof(MyProjectNameApplicationContractsModule),
        typeof(SnAbpIdentityApplicationModule),
        typeof(SnAbpPermissionManagementApplicationModule),
        typeof(SnAbpTenantManagementApplicationModule),
        typeof(SnAbpFeatureManagementApplicationModule),
        typeof(BpmApplicationModule),
        typeof(CmsApplicationModule),
        typeof(FileApplicationModule),
        typeof(CommonApplicationModule)
        )]
    public class MyProjectNameApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<MyProjectNameApplicationModule>();
            });
        }
    }
}
