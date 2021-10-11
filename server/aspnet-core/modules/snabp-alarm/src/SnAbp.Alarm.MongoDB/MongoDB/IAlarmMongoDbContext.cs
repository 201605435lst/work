using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Alarm.MongoDB
{
    [ConnectionStringName(AlarmDbProperties.ConnectionStringName)]
    public interface IAlarmMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
