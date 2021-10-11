using Volo.Abp.Modularity;

namespace SnAbp.Schedule
{
    [DependsOn(
        typeof(ScheduleDomainSharedModule)
        )]
    public class ScheduleDomainModule : AbpModule
    {

    }
}
