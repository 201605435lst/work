using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Regulation.EntityFrameworkCore
{
    public class RegulationModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public RegulationModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}