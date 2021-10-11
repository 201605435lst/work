using Volo.Abp.Modularity;

namespace SnAbp.ConstructionBase
{
    [DependsOn(
        typeof(ConstructionBaseApplicationModule),
        typeof(ConstructionBaseDomainTestModule)
        )]
    public class ConstructionBaseApplicationTestModule : AbpModule
    {

    }
}
