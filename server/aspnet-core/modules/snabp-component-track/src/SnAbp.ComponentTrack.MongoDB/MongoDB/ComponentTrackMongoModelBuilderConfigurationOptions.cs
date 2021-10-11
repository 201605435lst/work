using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.ComponentTrack.MongoDB
{
    public class ComponentTrackMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public ComponentTrackMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}