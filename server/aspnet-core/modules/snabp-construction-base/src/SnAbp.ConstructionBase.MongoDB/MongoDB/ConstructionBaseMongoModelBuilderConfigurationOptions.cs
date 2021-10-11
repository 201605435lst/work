using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.ConstructionBase.MongoDB
{
    public class ConstructionBaseMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public ConstructionBaseMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}