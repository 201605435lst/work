using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Construction.MongoDB
{
    [ConnectionStringName(ConstructionDbProperties.ConnectionStringName)]
    public interface IConstructionMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
