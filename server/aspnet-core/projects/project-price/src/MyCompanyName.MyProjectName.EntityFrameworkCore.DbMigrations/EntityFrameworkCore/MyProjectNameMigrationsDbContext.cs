using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.EntityFrameworkCore;
using SnAbp.Cms.EntityFrameworkCore;
using SnAbp.Common.EntityFrameworkCore;
using SnAbp.FeatureManagement.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.IdentityServer.EntityFrameworkCore;
using SnAbp.Message.Notice.EntityFrameworkCore;
using SnAbp.Message.Bpm.EntityFrameworkCore;
using SnAbp.PermissionManagement.EntityFrameworkCore;
using SnAbp.SettingManagement.EntityFrameworkCore;
using SnAbp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    /* This DbContext is only used for database migrations.
     * It is not used on runtime. See MyProjectNameDbContext for the runtime DbContext.
     * It is a unified model that includes configuration for
     * all used modules and your application.
     */
    public class MyProjectNameMigrationsDbContext : AbpDbContext<MyProjectNameMigrationsDbContext>
    {
        public MyProjectNameMigrationsDbContext(DbContextOptions<MyProjectNameMigrationsDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigureCommon();

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();
            builder.ConfigureIdentityServer();
            builder.ConfigureFeatureManagement();
            builder.ConfigureTenantManagement();          
            /* Configure your own tables/entities inside the ConfigureMyProjectName method */

            builder.ConfigureMyProjectName();
            builder.ConfigureFile();
            builder.ConfigureBpm();
            builder.ConfigureCms();
            builder.ConfigureNotice();
            builder.ConfigureNotice();
            builder.ConfigureMessageBpm();
        }
    }
}
