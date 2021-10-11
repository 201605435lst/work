using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Cms.EntityFrameworkCore
{
    public class CmsModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public CmsModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}