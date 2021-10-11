using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Message.Bpm.EntityFrameworkCore
{
    public class BpmMessageModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public BpmMessageModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}