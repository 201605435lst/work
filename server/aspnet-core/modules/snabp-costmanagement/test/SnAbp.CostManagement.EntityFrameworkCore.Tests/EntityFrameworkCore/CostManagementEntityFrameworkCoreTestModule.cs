using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace SnAbp.CostManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(CostManagementTestBaseModule),
        typeof(CostManagementEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
        )]
    public class CostManagementEntityFrameworkCoreTestModule : AbpModule
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
        
        private static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            new CostManagementDbContext(
                new DbContextOptionsBuilder<CostManagementDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();
            
            return connection;
        }
    }
}
