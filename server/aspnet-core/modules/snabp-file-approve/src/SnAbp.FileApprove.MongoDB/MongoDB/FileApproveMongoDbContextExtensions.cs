using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.FileApprove.MongoDB
{
    public static class FileApproveMongoDbContextExtensions
    {
        public static void ConfigureFileApprove(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new FileApproveMongoModelBuilderConfigurationOptions(
                FileApproveDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}