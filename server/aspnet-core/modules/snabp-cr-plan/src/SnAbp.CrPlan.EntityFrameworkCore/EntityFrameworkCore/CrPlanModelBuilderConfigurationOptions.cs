using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.CrPlan.EntityFrameworkCore
{
    public class CrPlanModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public CrPlanModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}