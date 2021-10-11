using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Alarm.MongoDB
{
    public static class AlarmMongoDbContextExtensions
    {
        public static void ConfigureAlarm(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new AlarmMongoModelBuilderConfigurationOptions(
                AlarmDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}