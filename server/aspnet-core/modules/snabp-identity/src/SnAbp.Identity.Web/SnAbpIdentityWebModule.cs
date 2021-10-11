using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using SnAbp.Identity.Localization;
using SnAbp.Identity.Web.Navigation;
using Volo.Abp.Modularity;
using SnAbp.PermissionManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Identity.Web
{
    [DependsOn(typeof(SnAbpIdentityHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpPermissionManagementWebModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiThemeSharedModule))]
    public class SnAbpIdentityWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(IdentityResource), typeof(SnAbpIdentityWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SnAbpIdentityWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpIdentityWebMainMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SnAbpIdentityWebModule>("Volo.Abp.Identity.Web");
            });

            context.Services.AddAutoMapperObjectMapper<SnAbpIdentityWebModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<SnAbpIdentityWebAutoMapperProfile>(validate: true);
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Identity/Users/Index", IdentityPermissions.Users.Default);
                options.Conventions.AuthorizePage("/Identity/Users/CreateModal", IdentityPermissions.Users.Create);
                options.Conventions.AuthorizePage("/Identity/Users/EditModal", IdentityPermissions.Users.Update);
                options.Conventions.AuthorizePage("/Identity/Roles/Index", IdentityPermissions.Roles.Default);
                options.Conventions.AuthorizePage("/Identity/Roles/CreateModal", IdentityPermissions.Roles.Create);
                options.Conventions.AuthorizePage("/Identity/Roles/EditModal", IdentityPermissions.Roles.Update);
            });
        }
    }
}