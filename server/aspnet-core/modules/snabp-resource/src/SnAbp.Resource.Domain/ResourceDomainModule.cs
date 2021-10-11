using Volo.Abp.Modularity;

namespace SnAbp.Resource
{
    [DependsOn(
        typeof(ResourceDomainSharedModule)
        )]
    public class ResourceDomainModule : AbpModule
    {

    }
}
