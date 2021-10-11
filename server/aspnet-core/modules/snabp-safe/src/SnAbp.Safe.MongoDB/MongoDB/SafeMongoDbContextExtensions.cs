using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Safe.MongoDB
{
    public static class SafeMongoDbContextExtensions
    {
        public static void ConfigureSafe(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new SafeMongoModelBuilderConfigurationOptions(
                SafeDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}