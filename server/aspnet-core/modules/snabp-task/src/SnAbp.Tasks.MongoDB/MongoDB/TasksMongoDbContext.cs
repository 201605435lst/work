using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Tasks.MongoDB
{
    [ConnectionStringName(TasksDbProperties.ConnectionStringName)]
    public class TasksMongoDbContext : AbpMongoDbContext, ITasksMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureTasks();
        }
    }
}