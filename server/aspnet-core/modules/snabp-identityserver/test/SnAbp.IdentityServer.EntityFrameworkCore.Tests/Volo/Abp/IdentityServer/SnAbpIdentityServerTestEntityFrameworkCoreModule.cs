using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp;

namespace SnAbp.IdentityServer
{
    [DependsOn(
        typeof(SnAbpIdentityEntityFrameworkCoreModule),
        typeof(SnAbpIdentityServerEntityFrameworkCoreModule),
        typeof(SnAbpIdentityServerTestBaseModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
        )]
    public class SnAbpIdentityServerTestEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var sqliteConnection = CreateDatabaseAndGetConnection();

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection);
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            new IdentityDbContext(
                new DbContextOptionsBuilder<IdentityDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();

            new IdentityServerDbContext(
                new DbContextOptionsBuilder<IdentityServerDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();

            return connection;
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                AsyncHelper.RunSync(() => scope.ServiceProvider
                    .GetRequiredService<SnAbpIdentityServerTestDataBuilder>()
                    .BuildAsync());
            }
        }
    }
}