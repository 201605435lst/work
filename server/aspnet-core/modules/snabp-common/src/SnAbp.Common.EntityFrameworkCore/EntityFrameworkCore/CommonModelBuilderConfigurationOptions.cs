using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Common.EntityFrameworkCore
{
    public class CommonModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public CommonModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}