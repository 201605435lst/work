using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Alarm.MongoDB
{
    [ConnectionStringName(AlarmDbProperties.ConnectionStringName)]
    public class AlarmMongoDbContext : AbpMongoDbContext, IAlarmMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureAlarm();
        }
    }
}