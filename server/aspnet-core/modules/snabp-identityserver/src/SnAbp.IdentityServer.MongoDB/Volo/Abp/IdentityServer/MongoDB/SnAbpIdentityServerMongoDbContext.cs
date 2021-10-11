using MongoDB.Driver;
using Volo.Abp.Data;
using SnAbp.IdentityServer.ApiResources;
using SnAbp.IdentityServer.Clients;
using SnAbp.IdentityServer.Devices;
using SnAbp.IdentityServer.Grants;
using Volo.Abp.MongoDB;
using IdentityResource = SnAbp.IdentityServer.IdentityResources.IdentityResource;

namespace SnAbp.IdentityServer.MongoDB
{
    [ConnectionStringName(SnAbpIdentityServerDbProperties.ConnectionStringName)]
    public class SnAbpIdentityServerMongoDbContext : AbpMongoDbContext, ISnAbpIdentityServerMongoDbContext
    {
        public IMongoCollection<ApiResource> ApiResources => Collection<ApiResource>();

        public IMongoCollection<Client> Clients => Collection<Client>();

        public IMongoCollection<IdentityResource> IdentityResources => Collection<IdentityResource>();

        public IMongoCollection<PersistedGrant> PersistedGrants => Collection<PersistedGrant>();

        public IMongoCollection<DeviceFlowCodes> DeviceFlowCodes => Collection<DeviceFlowCodes>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureIdentityServer();
        }
    }
}
