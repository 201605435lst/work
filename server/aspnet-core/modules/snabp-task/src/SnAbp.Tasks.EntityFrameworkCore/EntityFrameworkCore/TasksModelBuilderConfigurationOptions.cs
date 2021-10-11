using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Tasks.EntityFrameworkCore
{
    public class TasksModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public TasksModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}