using Volo.Abp.Modularity;

namespace SnAbp.StdBasic
{
    [DependsOn(
        typeof(StdBasicDomainSharedModule)
        )]
    public class StdBasicDomainModule : AbpModule
    {

    }
}
