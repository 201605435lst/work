using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace SnAbp.IdentityServer
{
    public class SnAbpIdentityServerTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
