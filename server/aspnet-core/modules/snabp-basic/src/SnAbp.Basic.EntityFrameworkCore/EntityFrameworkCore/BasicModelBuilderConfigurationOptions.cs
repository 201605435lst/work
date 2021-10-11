using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Basic.EntityFrameworkCore
{
    public class BasicModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public BasicModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}