using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.FileApprove.MongoDB
{
    public class FileApproveMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public FileApproveMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}