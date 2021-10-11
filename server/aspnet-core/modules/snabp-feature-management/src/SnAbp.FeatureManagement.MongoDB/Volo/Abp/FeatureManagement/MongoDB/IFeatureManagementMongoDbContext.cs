using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.FeatureManagement.MongoDB
{
    [ConnectionStringName(FeatureManagementDbProperties.ConnectionStringName)]
    public interface IFeatureManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<FeatureValue> FeatureValues { get; }
    }
}
