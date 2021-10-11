using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AutoMapper;
using SnAbp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Account.Web
{
    [DependsOn(
        typeof(SnAbpAccountHttpApiModule),
        typeof(SnAbpIdentityAspNetCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule)
        )]
    public class SnAbpAccountWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AccountResource), typeof(SnAbpAccountWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SnAbpAccountWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SnAbpAccountWebModule>("SnAbp.Account.Web");
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpAccountUserMenuContributor());
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new AccountModuleToolbarContributor());
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Account/Manage");
            });

            context.Services.AddAutoMapperObjectMapper<SnAbpAccountWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpAccountWebAutoMapperProfile>(validate: true);
            });
        }
    }
}
