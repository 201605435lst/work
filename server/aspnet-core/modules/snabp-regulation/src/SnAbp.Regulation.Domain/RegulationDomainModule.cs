using Volo.Abp.Modularity;

namespace SnAbp.Regulation
{
    [DependsOn(
        typeof(RegulationDomainSharedModule)
        )]
    public class RegulationDomainModule : AbpModule
    {

    }
}
