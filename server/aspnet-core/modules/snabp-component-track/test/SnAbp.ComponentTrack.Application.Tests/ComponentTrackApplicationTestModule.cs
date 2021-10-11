using Volo.Abp.Modularity;

namespace SnAbp.ComponentTrack
{
    [DependsOn(
        typeof(ComponentTrackApplicationModule),
        typeof(ComponentTrackDomainTestModule)
        )]
    public class ComponentTrackApplicationTestModule : AbpModule
    {

    }
}
