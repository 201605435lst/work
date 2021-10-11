using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.ConstructionBase.MongoDB
{
    [ConnectionStringName(ConstructionBaseDbProperties.ConnectionStringName)]
    public interface IConstructionBaseMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
