using Volo.Abp.Modularity;

namespace SnAbp.Emerg
{
    [DependsOn(
        typeof(EmergApplicationModule),
        typeof(EmergDomainTestModule)
        )]
    public class EmergApplicationTestModule : AbpModule
    {

    }
}
