using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.StdBasic.EntityFrameworkCore
{
    public class StdBasicModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public StdBasicModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}