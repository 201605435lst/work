using Volo.Abp.Modularity;

namespace SnAbp.Oa
{
    [DependsOn(
        typeof(OaApplicationModule),
        typeof(OaDomainTestModule)
        )]
    public class OaApplicationTestModule : AbpModule
    {

    }
}
