using Volo.Abp.Modularity;

namespace SnAbp.Alarm
{
    [DependsOn(
        typeof(AlarmApplicationModule),
        typeof(AlarmDomainTestModule)
        )]
    public class AlarmApplicationTestModule : AbpModule
    {

    }
}
