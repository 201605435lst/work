using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Oa.MongoDB
{
    public static class OaMongoDbContextExtensions
    {
        public static void ConfigureOa(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new OaMongoModelBuilderConfigurationOptions(
                OaDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}