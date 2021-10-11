using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.ComponentTrack
{
    [DependsOn(
        typeof(ComponentTrackDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class ComponentTrackApplicationContractsModule : AbpModule
    {

    }
}
