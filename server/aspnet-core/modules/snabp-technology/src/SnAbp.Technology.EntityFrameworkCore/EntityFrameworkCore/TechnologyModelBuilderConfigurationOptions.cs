using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Technology.EntityFrameworkCore
{
    public class TechnologyModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public TechnologyModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}