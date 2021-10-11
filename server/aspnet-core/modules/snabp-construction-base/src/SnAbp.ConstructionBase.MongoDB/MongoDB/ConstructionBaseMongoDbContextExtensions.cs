using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.ConstructionBase.MongoDB
{
    public static class ConstructionBaseMongoDbContextExtensions
    {
        public static void ConfigureConstructionBase(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ConstructionBaseMongoModelBuilderConfigurationOptions(
                ConstructionBaseDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}