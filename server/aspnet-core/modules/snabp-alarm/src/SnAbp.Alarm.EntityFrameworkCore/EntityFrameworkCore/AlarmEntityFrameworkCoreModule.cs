using Microsoft.Extensions.DependencyInjection;
using SnAbp.Alarm.EntityFrameworkCore.Repositories;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Resource.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Alarm.EntityFrameworkCore
{
    [DependsOn(
        typeof(AlarmDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(ResourceEntityFrameworkCoreModule),
        typeof(SnAbpIdentityEntityFrameworkCoreModule)
    )]
    public class AlarmEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AlarmDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */

                options.AddRepository<Entities.Alarm, EfCoreAlarmRepository>();
                options.AddRepository<Entities.AlarmEquipmentIdBind, EfCoreAlarmEquipmentBindIdRepository>();
            });

        }
    }
}