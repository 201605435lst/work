using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Technology.MongoDB
{
    public class TechnologyMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public TechnologyMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}