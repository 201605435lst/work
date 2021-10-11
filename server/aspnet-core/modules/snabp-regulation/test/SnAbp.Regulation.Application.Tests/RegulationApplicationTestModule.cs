using Volo.Abp.Modularity;

namespace SnAbp.Regulation
{
    [DependsOn(
        typeof(RegulationApplicationModule),
        typeof(RegulationDomainTestModule)
        )]
    public class RegulationApplicationTestModule : AbpModule
    {

    }
}
