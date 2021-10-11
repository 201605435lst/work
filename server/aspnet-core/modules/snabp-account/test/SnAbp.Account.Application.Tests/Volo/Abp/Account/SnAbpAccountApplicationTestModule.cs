using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using SnAbp.Identity.AspNetCore;
using SnAbp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using SnAbp.PermissionManagement.EntityFrameworkCore;

namespace SnAbp.Account
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAuthorizationModule),
        typeof(SnAbpIdentityAspNetCoreModule),
        typeof(SnAbpAccountApplicationModule),
        typeof(SnAbpIdentityEntityFrameworkCoreModule),
        typeof(SnAbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
    public class SnAbpAccountApplicationTestModule : AbpModule
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

            new IdentityDbContext(
                new DbContextOptionsBuilder<IdentityDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();

            new PermissionManagementDbContext(
                new DbContextOptionsBuilder<PermissionManagementDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();


            return connection;
        }
    }
}
