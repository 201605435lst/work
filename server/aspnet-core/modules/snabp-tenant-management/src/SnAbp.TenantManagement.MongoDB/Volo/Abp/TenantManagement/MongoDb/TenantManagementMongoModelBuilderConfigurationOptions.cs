using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace SnAbp.TenantManagement.MongoDB
{
    public class TenantManagementMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public TenantManagementMongoModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "")
            : base(tablePrefix)
        {
        }
    }
}