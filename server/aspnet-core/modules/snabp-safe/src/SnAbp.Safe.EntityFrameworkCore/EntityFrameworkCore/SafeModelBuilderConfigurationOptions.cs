using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Safe.EntityFrameworkCore
{
    public class SafeModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public SafeModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}