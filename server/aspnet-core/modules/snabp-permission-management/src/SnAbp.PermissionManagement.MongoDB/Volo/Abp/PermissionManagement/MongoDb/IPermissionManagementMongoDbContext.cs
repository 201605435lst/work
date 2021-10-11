using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.PermissionManagement.MongoDB
{
    [ConnectionStringName(SnAbpPermissionManagementDbProperties.ConnectionStringName)]
    public interface IPermissionManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<PermissionGrant> PermissionGrants { get; }
    }
}