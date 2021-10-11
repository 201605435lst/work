using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Project.MongoDB
{
    public static class ProjectMongoDbContextExtensions
    {
        public static void ConfigureProject(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ProjectMongoModelBuilderConfigurationOptions(
                ProjectDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}