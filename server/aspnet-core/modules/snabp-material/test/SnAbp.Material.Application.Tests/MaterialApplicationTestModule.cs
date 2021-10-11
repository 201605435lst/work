using Volo.Abp.Modularity;

namespace SnAbp.Material
{
    [DependsOn(
        typeof(MaterialApplicationModule),
        typeof(MaterialDomainTestModule)
        )]
    public class MaterialApplicationTestModule : AbpModule
    {

    }
}
