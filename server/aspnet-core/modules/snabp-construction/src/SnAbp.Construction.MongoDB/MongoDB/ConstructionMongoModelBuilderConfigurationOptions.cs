using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Construction.MongoDB
{
    public class ConstructionMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public ConstructionMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}