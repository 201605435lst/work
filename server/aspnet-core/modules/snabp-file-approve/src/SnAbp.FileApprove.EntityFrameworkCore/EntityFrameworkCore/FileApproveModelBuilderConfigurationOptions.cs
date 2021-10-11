using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.FileApprove.EntityFrameworkCore
{
    public class FileApproveModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public FileApproveModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}