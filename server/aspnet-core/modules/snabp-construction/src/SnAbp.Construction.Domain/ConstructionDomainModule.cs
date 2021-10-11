using Volo.Abp.Modularity;

namespace SnAbp.Construction
{
    [DependsOn(
        typeof(ConstructionDomainSharedModule)
        )]
    public class ConstructionDomainModule : AbpModule
    {

    }
}
