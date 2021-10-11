using System;
using SnAbp.IdentityServer.ApiResources;
using SnAbp.IdentityServer.Clients;
using SnAbp.IdentityServer.Devices;
using SnAbp.IdentityServer.Grants;
using SnAbp.IdentityServer.IdentityResources;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.IdentityServer.MongoDB
{
    public static class SnAbpIdentityServerMongoDbContextExtensions
    {
        public static void ConfigureIdentityServer(
            this IMongoModelBuilder builder,
            Action<IdentityServerMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new IdentityServerMongoModelBuilderConfigurationOptions(
                SnAbpIdentityServerDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<ApiResource>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "ApiResources";
            });

            builder.Entity<Client>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Clients";
            });
            builder.Entity<IdentityResource>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "IdentityResources";
            });

            builder.Entity<PersistedGrant>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "PersistedGrants";
            });

            builder.Entity<DeviceFlowCodes>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "DeviceFlowCodes";
            });
        }
    }
}
