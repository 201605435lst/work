using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Technology.MongoDB
{
    public static class TechnologyMongoDbContextExtensions
    {
        public static void ConfigureTechnology(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new TechnologyMongoModelBuilderConfigurationOptions(
                TechnologyDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}