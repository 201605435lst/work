using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.Tasks.MongoDB
{
    public class TasksMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public TasksMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}