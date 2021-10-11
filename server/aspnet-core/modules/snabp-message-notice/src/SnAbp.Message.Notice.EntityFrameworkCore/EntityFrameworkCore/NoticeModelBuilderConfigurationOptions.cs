using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Message.Notice.EntityFrameworkCore
{
    public class NoticeModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public NoticeModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}