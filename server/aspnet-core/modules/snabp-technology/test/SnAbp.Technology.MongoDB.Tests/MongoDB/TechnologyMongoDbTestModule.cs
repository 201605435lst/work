using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace SnAbp.Technology.MongoDB
{
    [DependsOn(
        typeof(TechnologyTestBaseModule),
        typeof(TechnologyMongoDbModule)
        )]
    public class TechnologyMongoDbTestModule : AbpModule
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