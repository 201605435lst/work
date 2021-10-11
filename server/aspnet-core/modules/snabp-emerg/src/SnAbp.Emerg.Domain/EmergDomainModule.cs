using Volo.Abp.Modularity;

namespace SnAbp.Emerg
{
    [DependsOn(
        typeof(EmergDomainSharedModule)
        )]
    public class EmergDomainModule : AbpModule
    {

    }
}
