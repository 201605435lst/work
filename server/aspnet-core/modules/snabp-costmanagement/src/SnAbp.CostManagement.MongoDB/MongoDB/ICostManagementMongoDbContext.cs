using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.CostManagement.MongoDB
{
    [ConnectionStringName(CostManagementDbProperties.ConnectionStringName)]
    public interface ICostManagementMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
