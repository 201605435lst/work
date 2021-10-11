using Volo.Abp.Modularity;

namespace SnAbp.Technology
{
    [DependsOn(
        typeof(TechnologyApplicationModule),
        typeof(TechnologyDomainTestModule)
        )]
    public class TechnologyApplicationTestModule : AbpModule
    {

    }
}
