using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.FeatureManagement.EntityFrameworkCore
{
    public class FeatureManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public FeatureManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}