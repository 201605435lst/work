using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Bpm.EntityFrameworkCore
{
    public class BpmModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public BpmModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}