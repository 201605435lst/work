using Volo.Abp.Modularity;

namespace SnAbp.Construction
{
    [DependsOn(
        typeof(ConstructionApplicationModule),
        typeof(ConstructionDomainTestModule)
        )]
    public class ConstructionApplicationTestModule : AbpModule
    {

    }
}
