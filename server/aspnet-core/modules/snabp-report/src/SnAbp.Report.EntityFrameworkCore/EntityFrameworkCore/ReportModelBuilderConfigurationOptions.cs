using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Report.EntityFrameworkCore
{
    public class ReportModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ReportModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}