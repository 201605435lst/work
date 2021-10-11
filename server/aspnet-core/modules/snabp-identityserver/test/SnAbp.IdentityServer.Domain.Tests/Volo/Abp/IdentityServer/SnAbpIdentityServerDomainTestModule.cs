using Volo.Abp.Modularity;

namespace SnAbp.IdentityServer
{
    [DependsOn(typeof(SnAbpIdentityServerTestEntityFrameworkCoreModule))]
    public class SnAbpIdentityServerDomainTestModule : AbpModule
    {

    }
}
