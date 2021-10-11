using Microsoft.Extensions.DependencyInjection;
using SnAbp.IdentityServer.Devices;
using SnAbp.IdentityServer.Grants;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using ApiResource = SnAbp.IdentityServer.ApiResources.ApiResource;
using Client = SnAbp.IdentityServer.Clients.Client;
using IdentityResource = SnAbp.IdentityServer.IdentityResources.IdentityResource;

namespace SnAbp.IdentityServer.MongoDB
{
    [DependsOn(
        typeof(SnAbpIdentityServerDomainModule),
        typeof(AbpMongoDbModule)
    )]
    public class SnAbpIdentityServerMongoDbModule : AbpModule
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
            context.Services.AddMongoDbContext<SnAbpIdentityServerMongoDbContext>(options =>
            {
                options.AddRepository<ApiResource, MongoApiResourceRepository>();
                options.AddRepository<IdentityResource, MongoIdentityResourceRepository>();
                options.AddRepository<Client, MongoClientRepository>();
                options.AddRepository<PersistedGrant, MongoPersistentGrantRepository>();
                options.AddRepository<DeviceFlowCodes, MongoDeviceFlowCodesRepository>();
            });
        }
    }
}
