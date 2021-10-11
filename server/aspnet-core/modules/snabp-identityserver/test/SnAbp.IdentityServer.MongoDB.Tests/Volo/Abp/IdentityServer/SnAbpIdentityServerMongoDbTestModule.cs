using System;
using Volo.Abp.Data;
using SnAbp.Identity.MongoDB;
using SnAbp.IdentityServer.MongoDB;
using Volo.Abp.Modularity;

namespace SnAbp.IdentityServer
{

    [DependsOn(
        typeof(SnAbpIdentityServerTestBaseModule),
        typeof(SnAbpIdentityServerMongoDbModule),
        typeof(SnAbpIdentityMongoDbModule)
    )]
    public class SnAbpIdentityServerMongoDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connectionString = MongoDbFixture.ConnectionString.EnsureEndsWith('/') +
                                   "Db_" +
                                   Guid.NewGuid().ToString("N");

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}
