using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.TenantManagement.EntityFrameworkCore
{
    [DependsOn(typeof(SnAbpTenantManagementDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class SnAbpTenantManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TenantManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<ITenantManagementDbContext>();
            });
        }
    }
}
