using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.TenantManagement.EntityFrameworkCore
{
    public class SnAbpTenantManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public SnAbpTenantManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}