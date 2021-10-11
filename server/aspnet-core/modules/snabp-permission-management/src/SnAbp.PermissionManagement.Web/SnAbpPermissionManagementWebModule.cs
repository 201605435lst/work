using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using SnAbp.PermissionManagement.HttpApi;
using SnAbp.PermissionManagement.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.PermissionManagement.Web
{
    [DependsOn(typeof(SnAbpPermissionManagementHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpPermissionManagementWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpPermissionManagementResource));
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpPermissionManagementWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpPermissionManagementWebModule>("SnAbp.PermissionManagement.Web");
            });

            context.Services.AddAutoMapperObjectMapper<AbpPermissionManagementWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<SnAbpPermissionManagementWebAutoMapperProfile>(validate: true);
            });
        }
    }
}
