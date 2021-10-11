using Volo.Abp;
using Volo.Abp.Testing;

namespace SnAbp.Account
{
    public class AbpAccountApplicationTestBase : AbpIntegratedTest<SnAbpAccountApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}