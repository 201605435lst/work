using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.ConstructionBase.EntityFrameworkCore
{
    public class ConstructionBaseModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ConstructionBaseModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}