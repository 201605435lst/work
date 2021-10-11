using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Regulation.MongoDB
{
    public class RegulationMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public RegulationMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}