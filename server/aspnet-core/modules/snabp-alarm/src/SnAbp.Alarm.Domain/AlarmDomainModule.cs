using Volo.Abp;
using Volo.Abp.Modularity;

namespace SnAbp.Alarm
{
    [DependsOn(
        typeof(AlarmDomainSharedModule)
        )]
    public class AlarmDomainModule : AbpModule
    {

    }
}
