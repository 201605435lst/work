using Volo.Abp.Modularity;

namespace SnAbp.Material
{
    [DependsOn(
        typeof(MaterialDomainSharedModule)
        )]
    public class MaterialDomainModule : AbpModule
    {

    }
}
