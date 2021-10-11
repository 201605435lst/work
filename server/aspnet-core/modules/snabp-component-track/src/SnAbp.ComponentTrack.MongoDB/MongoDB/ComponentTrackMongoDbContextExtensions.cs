using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.ComponentTrack.MongoDB
{
    public static class ComponentTrackMongoDbContextExtensions
    {
        public static void ConfigureComponentTrack(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ComponentTrackMongoModelBuilderConfigurationOptions(
                ComponentTrackDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}