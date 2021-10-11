using Volo.Abp.Modularity;

namespace SnAbp.Safe
{
    [DependsOn(
        typeof(SafeDomainSharedModule)
        )]
    public class SafeDomainModule : AbpModule
    {

    }
}
