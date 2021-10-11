using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using SnAbp.FeatureManagement;
using Volo.Abp.Modularity;
using SnAbp.TenantManagement.Localization;
using SnAbp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.TenantManagement.Web
{
    [DependsOn(typeof(SnAbpTenantManagementHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(SnAbpFeatureManagementWebModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class SnAbpTenantManagementWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpTenantManagementResource), typeof(SnAbpTenantManagementWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SnAbpTenantManagementWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new SnAbpTenantManagementWebMainMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SnAbpTenantManagementWebModule>("SnAbp.TenantManagement.Web");
            });

            context.Services.AddAutoMapperObjectMapper<SnAbpTenantManagementWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<SnAbpTenantManagementWebAutoMapperProfile>(validate: true);
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/Index", TenantManagementPermissions.Tenants.Default);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/CreateModal", TenantManagementPermissions.Tenants.Create);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/EditModal", TenantManagementPermissions.Tenants.Update);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/ConnectionStrings", TenantManagementPermissions.Tenants.ManageConnectionStrings);
            });
        }
    }
}