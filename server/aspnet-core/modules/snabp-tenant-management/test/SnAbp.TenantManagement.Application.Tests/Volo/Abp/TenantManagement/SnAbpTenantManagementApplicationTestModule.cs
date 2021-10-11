using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using SnAbp.TenantManagement.EntityFrameworkCore;

namespace SnAbp.TenantManagement
{
    [DependsOn(
        typeof(SnAbpTenantManagementApplicationModule), 
        typeof(SnAbpTenantManagementEntityFrameworkCoreTestModule))]
    public class SnAbpTenantManagementApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();
        }
    }
}
