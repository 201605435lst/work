using JetBrains.Annotations;

using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Oa.EntityFrameworkCore
{
    public class OaModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public OaModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}