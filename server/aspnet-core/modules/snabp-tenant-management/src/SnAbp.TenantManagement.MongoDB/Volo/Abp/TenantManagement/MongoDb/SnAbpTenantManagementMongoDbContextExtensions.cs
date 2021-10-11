using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.TenantManagement.MongoDB
{
    public static class SnAbpTenantManagementMongoDbContextExtensions
    {
        public static void ConfigureTenantManagement(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new TenantManagementMongoModelBuilderConfigurationOptions(
                SnAbpTenantManagementDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<Tenant>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Tenants";
            });
        }
    }
}