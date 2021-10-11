using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace SnAbp.Construction.MongoDB
{
    [DependsOn(
        typeof(ConstructionTestBaseModule),
        typeof(ConstructionMongoDbModule)
        )]
    public class ConstructionMongoDbTestModule : AbpModule
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