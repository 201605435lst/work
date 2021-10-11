using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace SnAbp.TenantManagement
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    [DependsOn(typeof(SnAbpTenantManagementDomainSharedModule))]
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class SnAbpTenantManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SnAbpTenantManagementDomainModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<SnAbpTenantManagementDomainMappingProfile>(validate: true);
            });

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<Tenant, TenantEto>();
            });
        }
    }
}
