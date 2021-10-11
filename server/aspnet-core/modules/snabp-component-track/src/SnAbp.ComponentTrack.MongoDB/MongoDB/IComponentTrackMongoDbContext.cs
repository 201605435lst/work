using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.ComponentTrack.MongoDB
{
    [ConnectionStringName(ComponentTrackDbProperties.ConnectionStringName)]
    public interface IComponentTrackMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
