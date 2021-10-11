using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.BackgroundJobs.EntityFrameworkCore
{
    public class BackgroundJobsModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public BackgroundJobsModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}