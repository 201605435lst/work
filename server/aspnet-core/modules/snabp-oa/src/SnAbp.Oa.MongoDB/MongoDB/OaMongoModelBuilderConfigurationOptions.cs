using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Oa.MongoDB
{
    public class OaMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public OaMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}