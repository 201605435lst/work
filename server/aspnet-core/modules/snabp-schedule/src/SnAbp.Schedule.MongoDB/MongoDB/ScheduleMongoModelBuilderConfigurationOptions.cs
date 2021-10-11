using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Schedule.MongoDB
{
    public class ScheduleMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public ScheduleMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}