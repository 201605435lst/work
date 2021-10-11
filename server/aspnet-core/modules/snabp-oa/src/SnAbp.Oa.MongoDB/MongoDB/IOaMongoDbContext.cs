using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Oa.MongoDB
{
    [ConnectionStringName(OaDbProperties.ConnectionStringName)]
    public interface IOaMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
