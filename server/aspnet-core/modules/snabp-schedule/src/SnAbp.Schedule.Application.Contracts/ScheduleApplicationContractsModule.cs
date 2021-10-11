using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Schedule
{
    [DependsOn(
        typeof(ScheduleDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class ScheduleApplicationContractsModule : AbpModule
    {

    }
}
