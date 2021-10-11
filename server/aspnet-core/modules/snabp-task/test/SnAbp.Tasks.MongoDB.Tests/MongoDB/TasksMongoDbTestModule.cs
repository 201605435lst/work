using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace SnAbp.Tasks.MongoDB
{
    [DependsOn(
        typeof(TasksTestBaseModule),
        typeof(TasksMongoDbModule)
        )]
    public class TasksMongoDbTestModule : AbpModule
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