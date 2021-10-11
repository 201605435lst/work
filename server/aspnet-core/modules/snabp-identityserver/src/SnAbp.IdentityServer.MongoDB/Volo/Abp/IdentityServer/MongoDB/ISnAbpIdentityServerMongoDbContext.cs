using MongoDB.Driver;
using Volo.Abp.Data;
using SnAbp.IdentityServer.Clients;
using SnAbp.IdentityServer.Devices;
using SnAbp.IdentityServer.Grants;
using SnAbp.IdentityServer.IdentityResources;
using Volo.Abp.MongoDB;
using ApiResource = SnAbp.IdentityServer.ApiResources.ApiResource;

namespace SnAbp.IdentityServer.MongoDB
{
    [ConnectionStringName(SnAbpIdentityServerDbProperties.ConnectionStringName)]
    public interface ISnAbpIdentityServerMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<ApiResource> ApiResources { get; }

        IMongoCollection<Client> Clients { get; }

        IMongoCollection<IdentityResource> IdentityResources { get; }

        IMongoCollection<PersistedGrant> PersistedGrants { get; }

        IMongoCollection<DeviceFlowCodes> DeviceFlowCodes { get; }
    }
}
