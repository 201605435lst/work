using Volo.Abp.Modularity;

namespace SnAbp.Resource
{
    [DependsOn(
        typeof(ResourceApplicationModule),
        typeof(ResourceDomainTestModule)
        )]
    public class ResourceApplicationTestModule : AbpModule
    {

    }
}
