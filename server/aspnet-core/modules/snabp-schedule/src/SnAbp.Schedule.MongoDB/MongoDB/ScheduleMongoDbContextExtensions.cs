using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Schedule.MongoDB
{
    public static class ScheduleMongoDbContextExtensions
    {
        public static void ConfigureSchedule(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ScheduleMongoModelBuilderConfigurationOptions(
                ScheduleDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}