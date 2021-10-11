using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.SettingManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(SnAbpSettingManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class SnAbpSettingManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<SettingManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<ISettingManagementDbContext>();

                options.AddRepository<Setting, EfCoreSettingRepository>();
            });

        }
    }
}
