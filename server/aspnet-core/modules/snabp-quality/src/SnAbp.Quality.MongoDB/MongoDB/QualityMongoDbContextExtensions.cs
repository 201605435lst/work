using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Quality.MongoDB
{
    public static class QualityMongoDbContextExtensions
    {
        public static void ConfigureQuality(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new QualityMongoModelBuilderConfigurationOptions(
                QualityDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}