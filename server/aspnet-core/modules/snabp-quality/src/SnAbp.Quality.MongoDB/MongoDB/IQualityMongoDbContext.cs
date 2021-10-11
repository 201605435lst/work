using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Quality.MongoDB
{
    [ConnectionStringName(QualityDbProperties.ConnectionStringName)]
    public interface IQualityMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
