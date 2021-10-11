using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Safe
{
    [DependsOn(
        typeof(SafeDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class SafeApplicationContractsModule : AbpModule
    {

    }
}
