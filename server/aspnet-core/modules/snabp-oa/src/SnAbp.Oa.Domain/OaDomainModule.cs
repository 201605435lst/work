using Volo.Abp.Modularity;

namespace SnAbp.Oa
{
    [DependsOn(
        typeof(OaDomainSharedModule)
        )]
    public class OaDomainModule : AbpModule
    {

    }
}
