using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.IdentityServer.Clients;
using SnAbp.IdentityServer.Devices;
using SnAbp.IdentityServer.Grants;

namespace SnAbp.IdentityServer
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddAbpStores(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();

            return builder
                .AddClientStore<ClientStore>()
                .AddResourceStore<ResourceStore>()
                .AddCorsPolicyService<SnAbpCorsPolicyService>();
        }
    }
}