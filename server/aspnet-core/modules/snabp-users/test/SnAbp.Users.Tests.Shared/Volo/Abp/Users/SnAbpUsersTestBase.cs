using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace SnAbp.Users
{
    public abstract class AbpUsersTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
