using Volo.Abp;
using Volo.Abp.Testing;

namespace SnAbp.IdentityServer
{
    public class SnAbpIdentityServerTestBase : AbpIntegratedTest<SnAbpIdentityServerTestEntityFrameworkCoreModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
