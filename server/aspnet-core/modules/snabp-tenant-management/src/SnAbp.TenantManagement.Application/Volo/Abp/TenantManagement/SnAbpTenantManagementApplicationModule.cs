using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace SnAbp.TenantManagement
{
    [DependsOn(typeof(SnAbpTenantManagementDomainModule))]
    [DependsOn(typeof(SnAbpTenantManagementApplicationContractsModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class SnAbpTenantManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SnAbpTenantManagementApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<SnAbpTenantManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}