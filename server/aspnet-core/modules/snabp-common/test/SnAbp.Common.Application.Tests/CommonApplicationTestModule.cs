using Volo.Abp.Modularity;

namespace SnAbp.Common
{
    [DependsOn(
        typeof(CommonApplicationModule),
        typeof(CommonDomainTestModule)
        )]
    public class CommonApplicationTestModule : AbpModule
    {

    }
}
