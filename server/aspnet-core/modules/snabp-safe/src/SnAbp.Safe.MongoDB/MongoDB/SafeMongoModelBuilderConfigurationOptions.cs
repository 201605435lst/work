using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Safe.MongoDB
{
    public class SafeMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public SafeMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}