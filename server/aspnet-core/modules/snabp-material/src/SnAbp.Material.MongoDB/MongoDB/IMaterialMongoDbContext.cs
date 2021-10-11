using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Material.MongoDB
{
    [ConnectionStringName(MaterialDbProperties.ConnectionStringName)]
    public interface IMaterialMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
