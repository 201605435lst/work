using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using SnAbp.IdentityServer.ApiResources;
using SnAbp.IdentityServer.Clients;
using SnAbp.IdentityServer.Devices;
using SnAbp.IdentityServer.Grants;
using SnAbp.IdentityServer.IdentityResources;
using Volo.Abp.Modularity;

namespace SnAbp.IdentityServer.EntityFrameworkCore
{
    [DependsOn(
        typeof(SnAbpIdentityServerDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class SnAbpIdentityServerEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<IIdentityServerBuilder>(
                builder =>
                {
                    builder.AddAbpStores();
                }
            );
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IdentityServerDbContext>(options =>
            {
                options.AddDefaultRepositories<IIdentityServerDbContext>();

                options.AddRepository<Client, ClientRepository>();
                options.AddRepository<ApiResource, ApiResourceRepository>();
                options.AddRepository<IdentityResource, IdentityResourceRepository>();
                options.AddRepository<PersistedGrant, PersistentGrantRepository>();
                options.AddRepository<DeviceFlowCodes, DeviceFlowCodesRepository>();
            });
        }
    }
}
