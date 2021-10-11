using Volo.Abp.Modularity;

namespace SnAbp.Bpm
{
    [DependsOn(
        typeof(BpmApplicationModule),
        typeof(BpmDomainTestModule)
        )]
    public class BpmApplicationTestModule : AbpModule
    {

    }
}
