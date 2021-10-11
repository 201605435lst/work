using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Regulation.MongoDB
{
    public static class RegulationMongoDbContextExtensions
    {
        public static void ConfigureRegulation(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new RegulationMongoModelBuilderConfigurationOptions(
                RegulationDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}