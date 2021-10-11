using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Report.MongoDB
{
    public class ReportMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public ReportMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}