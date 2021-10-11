using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.File2.MongoDB
{
    public static class File2MongoDbContextExtensions
    {
        public static void ConfigureFile2(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new File2MongoModelBuilderConfigurationOptions(
                File2DbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}