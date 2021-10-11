using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.File2.MongoDB
{
    public class File2MongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public File2MongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}