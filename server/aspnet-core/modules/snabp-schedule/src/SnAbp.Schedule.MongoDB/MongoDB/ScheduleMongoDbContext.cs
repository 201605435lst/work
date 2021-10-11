using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Schedule.MongoDB
{
    [ConnectionStringName(ScheduleDbProperties.ConnectionStringName)]
    public class ScheduleMongoDbContext : AbpMongoDbContext, IScheduleMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureSchedule();
        }
    }
}