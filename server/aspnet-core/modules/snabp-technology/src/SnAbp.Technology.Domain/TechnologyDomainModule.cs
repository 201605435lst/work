using Volo.Abp.Modularity;

namespace SnAbp.Technology
{
    [DependsOn(
        typeof(TechnologyDomainSharedModule)
        )]
    public class TechnologyDomainModule : AbpModule
    {

    }
}
