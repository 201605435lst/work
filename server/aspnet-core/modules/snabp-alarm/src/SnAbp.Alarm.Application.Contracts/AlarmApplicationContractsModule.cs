using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Alarm
{
    [DependsOn(
        typeof(AlarmDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class AlarmApplicationContractsModule : AbpModule
    {

    }
}
