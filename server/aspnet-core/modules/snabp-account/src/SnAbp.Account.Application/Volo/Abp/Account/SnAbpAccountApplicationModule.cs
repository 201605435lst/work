using SnAbp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Account
{
    [DependsOn(
        typeof(SnAbpAccountApplicationContractsModule),
        typeof(SnAbpIdentityApplicationModule),
        typeof(AbpUiNavigationModule)
    )]
    public class SnAbpAccountApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SnAbpAccountApplicationModule>();
            });
        }
    }
}