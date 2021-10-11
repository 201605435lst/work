using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.FileApprove.MongoDB
{
    [ConnectionStringName(FileApproveDbProperties.ConnectionStringName)]
    public interface IFileApproveMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
