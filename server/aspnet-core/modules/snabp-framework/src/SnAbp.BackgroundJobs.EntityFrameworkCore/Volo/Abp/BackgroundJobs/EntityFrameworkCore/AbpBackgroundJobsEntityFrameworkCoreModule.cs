using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.BackgroundJobs.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class AbpBackgroundJobsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BackgroundJobsDbContext>(options =>
            {
                options.AddRepository<BackgroundJobRecord, EfCoreBackgroundJobRepository>();
            });
        }
    }
}