using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.CostManagement.MongoDB
{
    public static class CostManagementMongoDbContextExtensions
    {
        public static void ConfigureCostManagement(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new CostManagementMongoModelBuilderConfigurationOptions(
                CostManagementDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}