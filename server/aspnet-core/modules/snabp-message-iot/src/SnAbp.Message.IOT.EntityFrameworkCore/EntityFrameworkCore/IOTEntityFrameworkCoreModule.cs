using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Message.IOT.EntityFrameworkCore
{
    [DependsOn(
        typeof(IOTDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class IOTEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IOTDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });
        }
    }
}