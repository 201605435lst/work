using Volo.Abp.Modularity;

namespace SnAbp.Basic
{
    [DependsOn(
        typeof(BasicDomainSharedModule)
        )]
    public class BasicDomainModule : AbpModule
    {

    }
}
