﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace SnAbp.IdentityServer
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(SnAbpIdentityServerDomainModule)
    )]
    public class SnAbpIdentityServerTestBaseModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<SnAbpIdentityServerBuilderOptions>(options =>
            {
                options.AddDeveloperSigningCredential = false;
            });

            PreConfigure<IIdentityServerBuilder>(identityServerBuilder =>
            {
                identityServerBuilder.AddDeveloperSigningCredential(false, System.Guid.NewGuid().ToString());
            });
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                AsyncHelper.RunSync(() => scope.ServiceProvider
                    .GetRequiredService<SnAbpIdentityServerTestDataBuilder>()
                    .BuildAsync());
            }
        }
    }
}
