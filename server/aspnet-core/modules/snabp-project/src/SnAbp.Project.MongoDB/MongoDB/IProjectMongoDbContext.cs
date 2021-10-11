using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Project.MongoDB
{
    [ConnectionStringName(ProjectDbProperties.ConnectionStringName)]
    public interface IProjectMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
