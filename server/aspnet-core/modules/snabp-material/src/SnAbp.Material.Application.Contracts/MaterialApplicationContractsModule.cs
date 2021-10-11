using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Material
{
    [DependsOn(
        typeof(MaterialDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class MaterialApplicationContractsModule : AbpModule
    {

    }
}
