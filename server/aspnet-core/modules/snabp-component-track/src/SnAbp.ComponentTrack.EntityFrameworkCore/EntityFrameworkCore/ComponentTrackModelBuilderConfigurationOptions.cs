using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.ComponentTrack.EntityFrameworkCore
{
    public class ComponentTrackModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ComponentTrackModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}