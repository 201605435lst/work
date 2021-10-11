using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Quality.MongoDB
{
    public class QualityMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public QualityMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}