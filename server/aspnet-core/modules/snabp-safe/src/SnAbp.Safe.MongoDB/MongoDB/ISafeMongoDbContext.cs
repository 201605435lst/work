using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Safe.MongoDB
{
    [ConnectionStringName(SafeDbProperties.ConnectionStringName)]
    public interface ISafeMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
