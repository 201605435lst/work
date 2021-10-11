using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Material.EntityFrameworkCore
{
    public class MaterialModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public MaterialModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}