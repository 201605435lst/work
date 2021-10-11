using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Schedule.EntityFrameworkCore
{
    public class ScheduleModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ScheduleModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}