using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Construction
{
    [DependsOn(
        typeof(ConstructionDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class ConstructionApplicationContractsModule : AbpModule
    {

    }
}
