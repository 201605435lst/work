using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.FeatureManagement.MongoDB
{
    public class FeatureManagementMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public FeatureManagementMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {

        }
    }
}