using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Construction.EntityFrameworkCore
{
    public class ConstructionModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ConstructionModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}