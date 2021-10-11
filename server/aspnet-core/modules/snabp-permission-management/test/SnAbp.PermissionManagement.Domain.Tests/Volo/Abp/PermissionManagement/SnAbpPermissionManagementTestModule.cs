using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using SnAbp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace SnAbp.PermissionManagement
{
    [DependsOn(
        typeof(SnAbpPermissionManagementEntityFrameworkCoreModule), 
        typeof(AbpPermissionManagementTestBaseModule))]
    public class AbpPermissionManagementTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEntityFrameworkInMemoryDatabase();

            var databaseName = Guid.NewGuid().ToString();

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions.UseInMemoryDatabase(databaseName);
                });
            });

            Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled; //EF in-memory database does not support transactions
            });
        }
    }
}
