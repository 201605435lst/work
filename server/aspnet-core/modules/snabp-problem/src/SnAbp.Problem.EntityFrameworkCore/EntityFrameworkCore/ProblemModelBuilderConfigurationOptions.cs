using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Problem.EntityFrameworkCore
{
    public class ProblemModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ProblemModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}