using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Schedule.MongoDB
{
    [ConnectionStringName(ScheduleDbProperties.ConnectionStringName)]
    public interface IScheduleMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
