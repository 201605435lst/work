using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Tasks.MongoDB
{
    public static class TasksMongoDbContextExtensions
    {
        public static void ConfigureTasks(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new TasksMongoModelBuilderConfigurationOptions(
                TasksDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}