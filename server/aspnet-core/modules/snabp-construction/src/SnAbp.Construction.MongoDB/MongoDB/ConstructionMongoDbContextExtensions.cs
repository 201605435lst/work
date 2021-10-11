using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Construction.MongoDB
{
    public static class ConstructionMongoDbContextExtensions
    {
        public static void ConfigureConstruction(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ConstructionMongoModelBuilderConfigurationOptions(
                ConstructionDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}