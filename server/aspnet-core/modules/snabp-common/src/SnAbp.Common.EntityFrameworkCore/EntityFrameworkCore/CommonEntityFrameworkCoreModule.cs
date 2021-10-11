using Microsoft.Extensions.DependencyInjection;
//using SnAbp.Common.Eitities;
//using SnAbp.Common.Repositories;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Common.EntityFrameworkCore
{
    [DependsOn(
        typeof(CommonDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class CommonEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CommonDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<ICommonDbContext>(true);
            });
        }
    }
}