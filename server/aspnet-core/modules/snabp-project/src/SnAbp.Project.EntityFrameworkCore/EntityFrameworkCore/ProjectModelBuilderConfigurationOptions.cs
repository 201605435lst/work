using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Project.EntityFrameworkCore
{
    public class ProjectModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ProjectModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}