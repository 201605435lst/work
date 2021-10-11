using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using SnAbp.PermissionManagement;

namespace SnAbp.Identity
{
    [DependsOn(
        typeof(SnAbpIdentityDomainModule),
        typeof(AbpIdentityApplicationContractsModule), 
        typeof(AbpAutoMapperModule),
        typeof(SnAbpPermissionManagementApplicationModule)
        )]
    public class SnAbpIdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SnAbpIdentityApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                //options.AddProfile<SnAbpIdentityApplicationModuleAutoMapperProfile>(validate: true);
                options.AddMaps<SnAbpIdentityApplicationModule>();
            });
        }
    }
}