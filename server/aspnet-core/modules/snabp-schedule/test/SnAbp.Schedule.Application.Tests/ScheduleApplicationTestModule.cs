using Volo.Abp.Modularity;

namespace SnAbp.Schedule
{
    [DependsOn(
        typeof(ScheduleApplicationModule),
        typeof(ScheduleDomainTestModule)
        )]
    public class ScheduleApplicationTestModule : AbpModule
    {

    }
}
