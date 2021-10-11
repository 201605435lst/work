using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Alarm.MongoDB
{
    public class AlarmMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public AlarmMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}