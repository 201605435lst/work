using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.CostManagement.EntityFrameworkCore
{
    public class CostManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public CostManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}