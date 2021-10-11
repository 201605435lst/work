using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Resource.EntityFrameworkCore
{
    public class ResourceModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ResourceModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}