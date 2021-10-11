using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.PermissionManagement.EntityFrameworkCore
{
    [DependsOn(typeof(SnAbpPermissionManagementDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class SnAbpPermissionManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PermissionManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<IPermissionManagementDbContext>();

                options.AddRepository<PermissionGrant, EfCorePermissionGrantRepository>();
            });
        }
    }
}
