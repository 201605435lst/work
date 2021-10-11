using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.FeatureManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(SnAbpFeatureManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class SnAbpFeatureManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FeatureManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<IFeatureManagementDbContext>();

                options.AddRepository<FeatureValue, EfCoreFeatureValueRepository>();
            });
        }
    }
}