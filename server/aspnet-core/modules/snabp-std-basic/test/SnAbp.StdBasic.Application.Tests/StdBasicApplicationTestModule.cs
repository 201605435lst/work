using Volo.Abp.Modularity;

namespace SnAbp.StdBasic
{
    [DependsOn(
        typeof(StdBasicApplicationModule),
        typeof(StdBasicDomainTestModule)
        )]
    public class StdBasicApplicationTestModule : AbpModule
    {

    }
}
