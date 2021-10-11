using Volo.Abp.Modularity;

namespace SnAbp.Common
{
    [DependsOn(
        typeof(CommonDomainSharedModule)
        )]
    public class CommonDomainModule : AbpModule
    {

    }
}
