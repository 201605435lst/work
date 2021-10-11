using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Message.IOT.EntityFrameworkCore
{
    public class IOTModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public IOTModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}