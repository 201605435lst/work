using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.File2.MongoDB
{
    [ConnectionStringName(File2DbProperties.ConnectionStringName)]
    public interface IFile2MongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
