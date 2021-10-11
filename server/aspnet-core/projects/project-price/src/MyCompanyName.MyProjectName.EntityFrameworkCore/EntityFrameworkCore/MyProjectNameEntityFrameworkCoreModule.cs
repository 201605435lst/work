using Microsoft.Extensions.DependencyInjection;
using SnAbp.Bpm.EntityFrameworkCore;
using SnAbp.Cms.EntityFrameworkCore;
using SnAbp.Common.EntityFrameworkCore;
using SnAbp.FeatureManagement.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.IdentityServer.EntityFrameworkCore;
using SnAbp.Message.Notice.EntityFrameworkCore;
using SnAbp.PermissionManagement.EntityFrameworkCore;
using SnAbp.Message.Bpm.EntityFrameworkCore;
using SnAbp.SettingManagement.EntityFrameworkCore;
using SnAbp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;
using MyCompanyName.MyProjectName.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule),
        typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(CommonEntityFrameworkCoreModule),
        typeof(SnAbpIdentityEntityFrameworkCoreModule),
        typeof(SnAbpIdentityServerEntityFrameworkCoreModule),
        typeof(SnAbpPermissionManagementEntityFrameworkCoreModule),
        typeof(SnAbpSettingManagementEntityFrameworkCoreModule),
        typeof(SnAbpTenantManagementEntityFrameworkCoreModule),
        typeof(SnAbpFeatureManagementEntityFrameworkCoreModule),
        typeof(BpmEntityFrameworkCoreModule),
        typeof(CmsEntityFrameworkCoreModule),
        typeof(FileEntityFrameworkCoreModule),
        typeof(NoticeEntityFrameworkCoreModule),
        typeof(NoticeEntityFrameworkCoreModule),
        typeof(BpmMessageEntityFrameworkCoreModule)
        )]
    public class MyProjectNameEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            MyProjectNameEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyProjectNameDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);


                options.Entity<Project>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.ProjectRltModules).ThenInclude(y => y.Module)
                );

                options.Entity<Module>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.Children)
                );

            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also MyProjectNameMigrationsDbContextFactory for EF Core tooling. */
                options.UseNpgsql();
            });
        }
    }
}
