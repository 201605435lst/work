using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace SnAbp.FileApprove.MongoDB
{
    [DependsOn(
        typeof(FileApproveTestBaseModule),
        typeof(FileApproveMongoDbModule)
        )]
    public class FileApproveMongoDbTestModule : AbpModule
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