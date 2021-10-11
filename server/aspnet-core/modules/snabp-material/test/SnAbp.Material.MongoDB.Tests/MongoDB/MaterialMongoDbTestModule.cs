using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace SnAbp.Material.MongoDB
{
    [DependsOn(
        typeof(MaterialTestBaseModule),
        typeof(MaterialMongoDbModule)
        )]
    public class MaterialMongoDbTestModule : AbpModule
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