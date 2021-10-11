using Volo.Abp;
using Volo.Abp.Testing;

namespace SnAbp.IdentityServer
{
    public class SnAbpIdentityServerDomainTestBase : AbpIntegratedTest<SnAbpIdentityServerDomainTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
