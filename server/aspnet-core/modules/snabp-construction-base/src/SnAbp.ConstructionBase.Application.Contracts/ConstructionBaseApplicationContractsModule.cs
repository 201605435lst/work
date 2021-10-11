using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.ConstructionBase
{
    [DependsOn(
        typeof(ConstructionBaseDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class ConstructionBaseApplicationContractsModule : AbpModule
    {

    }
}
