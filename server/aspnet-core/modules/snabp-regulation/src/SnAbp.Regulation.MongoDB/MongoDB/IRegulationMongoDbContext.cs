using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Regulation.MongoDB
{
    [ConnectionStringName(RegulationDbProperties.ConnectionStringName)]
    public interface IRegulationMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
