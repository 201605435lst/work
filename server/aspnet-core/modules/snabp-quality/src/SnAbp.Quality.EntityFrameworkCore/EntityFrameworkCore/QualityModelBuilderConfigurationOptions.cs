using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Quality.EntityFrameworkCore
{
    public class QualityModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public QualityModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}