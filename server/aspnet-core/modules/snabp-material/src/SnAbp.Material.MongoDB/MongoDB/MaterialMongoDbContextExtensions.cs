using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Material.MongoDB
{
    public static class MaterialMongoDbContextExtensions
    {
        public static void ConfigureMaterial(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new MaterialMongoModelBuilderConfigurationOptions(
                MaterialDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}