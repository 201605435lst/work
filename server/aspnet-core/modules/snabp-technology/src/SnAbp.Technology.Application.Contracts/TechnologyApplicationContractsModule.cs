using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Technology
{
    [DependsOn(
        typeof(TechnologyDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class TechnologyApplicationContractsModule : AbpModule
    {

    }
}
