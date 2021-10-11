using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Material.MongoDB
{
    public class MaterialMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public MaterialMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}