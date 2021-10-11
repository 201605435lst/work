using JetBrains.Annotations;

using SnAbp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class AbpAuditLoggingModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public AbpAuditLoggingModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix, 
                schema)
        {

        }
    }
}