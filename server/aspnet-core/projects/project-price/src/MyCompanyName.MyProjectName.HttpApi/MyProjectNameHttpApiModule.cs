using SnAbp.Account;
using SnAbp.Bpm;
using SnAbp.Cms;
using SnAbp.Common;
using SnAbp.FeatureManagement;
using SnAbp.File;
using SnAbp.Identity;
using SnAbp.Message.Notice;
using SnAbp.PermissionManagement.HttpApi;
using SnAbp.TenantManagement;
using Volo.Abp.Modularity;
using SnAbp.Message.Bpm;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationContractsModule),
        typeof(SnAbpAccountHttpApiModule),
        typeof(SnAbpIdentityHttpApiModule),
        typeof(SnAbpPermissionManagementHttpApiModule),
        typeof(SnAbpTenantManagementHttpApiModule),
        typeof(SnAbpFeatureManagementHttpApiModule),
        typeof(NoticeHttpApiModule),
        typeof(MessageBpmHttpApiModule),
        typeof(BpmHttpApiModule),
        typeof(CmsHttpApiModule),
        typeof(FileHttpApiModule),
        typeof(CommonHttpApiModule)
        )]
    public class MyProjectNameHttpApiModule : AbpModule
    {
    }
}
