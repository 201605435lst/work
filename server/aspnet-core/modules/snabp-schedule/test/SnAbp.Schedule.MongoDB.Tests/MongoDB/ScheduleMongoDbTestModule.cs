using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace SnAbp.Schedule.MongoDB
{
    [DependsOn(
        typeof(ScheduleTestBaseModule),
        typeof(ScheduleMongoDbModule)
        )]
    public class ScheduleMongoDbTestModule : AbpModule
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