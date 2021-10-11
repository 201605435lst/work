using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace SnAbp.Alarm.MongoDB
{
    [DependsOn(
        typeof(AlarmTestBaseModule),
        typeof(AlarmMongoDbModule)
        )]
    public class AlarmMongoDbTestModule : AbpModule
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