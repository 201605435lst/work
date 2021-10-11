using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.CostManagement.MongoDB
{
    public class CostManagementMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public CostManagementMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}