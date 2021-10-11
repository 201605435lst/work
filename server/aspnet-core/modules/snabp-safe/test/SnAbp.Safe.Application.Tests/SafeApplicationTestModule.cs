using Volo.Abp.Modularity;

namespace SnAbp.Safe
{
    [DependsOn(
        typeof(SafeApplicationModule),
        typeof(SafeDomainTestModule)
        )]
    public class SafeApplicationTestModule : AbpModule
    {

    }
}
