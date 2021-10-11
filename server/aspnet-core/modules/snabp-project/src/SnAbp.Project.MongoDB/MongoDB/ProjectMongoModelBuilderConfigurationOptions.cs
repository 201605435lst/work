using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Project.MongoDB
{
    public class ProjectMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public ProjectMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}