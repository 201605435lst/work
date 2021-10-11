using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace SnAbp.Quality
{
    [DependsOn(
        typeof(QualityDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class QualityApplicationContractsModule : AbpModule
    {

    }
}
