using Volo.Abp.Modularity;

namespace SnAbp.ComponentTrack
{
    [DependsOn(
        typeof(ComponentTrackDomainSharedModule)
        )]
    public class ComponentTrackDomainModule : AbpModule
    {

    }
}
