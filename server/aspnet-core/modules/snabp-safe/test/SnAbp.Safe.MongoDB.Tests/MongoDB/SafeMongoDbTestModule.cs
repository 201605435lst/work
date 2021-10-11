using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace SnAbp.Safe.MongoDB
{
    [DependsOn(
        typeof(SafeTestBaseModule),
        typeof(SafeMongoDbModule)
        )]
    public class SafeMongoDbTestModule : AbpModule
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