using Volo.Abp.Modularity;

namespace SnAbp.Cms
{
    [DependsOn(
        typeof(CmsDomainSharedModule)
        )]
    public class CmsDomainModule : AbpModule
    {

    }
}
