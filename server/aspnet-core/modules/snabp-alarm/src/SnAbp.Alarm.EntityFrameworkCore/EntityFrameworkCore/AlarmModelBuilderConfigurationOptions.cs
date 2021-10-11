using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Alarm.EntityFrameworkCore
{
    public class AlarmModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public AlarmModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}