using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.File.EntityFrameworkCore
{
    public class FileModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public FileModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {
        }
    }
}