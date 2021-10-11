using Volo.Abp.Modularity;

namespace SnAbp.Basic
{
    [DependsOn(
        typeof(BasicApplicationModule),
        typeof(BasicDomainTestModule)
        )]
    public class BasicApplicationTestModule : AbpModule
    {

    }
}
