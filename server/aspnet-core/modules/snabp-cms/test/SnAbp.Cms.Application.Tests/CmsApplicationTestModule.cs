using Volo.Abp.Modularity;

namespace SnAbp.Cms
{
    [DependsOn(
        typeof(CmsApplicationModule),
        typeof(CmsDomainTestModule)
        )]
    public class CmsApplicationTestModule : AbpModule
    {

    }
}
