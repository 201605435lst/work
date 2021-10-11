using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.PermissionManagement.EntityFrameworkCore
{
    public class SnAbpPermissionManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public SnAbpPermissionManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}