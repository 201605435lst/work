using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.ComponentTrack.EntityFrameworkCore
{
    [DependsOn(
        typeof(ComponentTrackDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class ComponentTrackEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ComponentTrackDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<IComponentTrackDbContext>(true);
               
            });
        }
    }
}