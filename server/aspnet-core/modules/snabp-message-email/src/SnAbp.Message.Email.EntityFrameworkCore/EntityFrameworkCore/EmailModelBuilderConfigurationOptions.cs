using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Message.Email.EntityFrameworkCore
{
    public class EmailModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public EmailModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}