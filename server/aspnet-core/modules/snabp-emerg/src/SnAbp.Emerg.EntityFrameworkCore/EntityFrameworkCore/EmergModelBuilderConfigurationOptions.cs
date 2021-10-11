using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Emerg.EntityFrameworkCore
{
    public class EmergModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public EmergModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}