using Microsoft.EntityFrameworkCore;
using SnAbp.Alarm.EntityFrameworkCore;
using SnAbp.Basic.EntityFrameworkCore;
using SnAbp.Bpm.EntityFrameworkCore;
using SnAbp.Cms.EntityFrameworkCore;
using SnAbp.Common.EntityFrameworkCore;
using SnAbp.CrPlan.EntityFrameworkCore;
//using SnAbp.Emerg.EntityFrameworkCore;
//using SnAbp.Exam.EntityFrameworkCore;
using SnAbp.FeatureManagement.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Tasks.EntityFrameworkCore;
using SnAbp.Report.EntityFrameworkCore;
using SnAbp.Regulation.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.IdentityServer.EntityFrameworkCore;
using SnAbp.Message.Notice.EntityFrameworkCore;
using SnAbp.Message.Bpm.EntityFrameworkCore;
using SnAbp.Oa.EntityFrameworkCore;
using SnAbp.PermissionManagement.EntityFrameworkCore;
using SnAbp.Problem.EntityFrameworkCore;
using SnAbp.Project.EntityFrameworkCore;
using SnAbp.Resource.EntityFrameworkCore;
using SnAbp.Schedule.EntityFrameworkCore;
using SnAbp.SettingManagement.EntityFrameworkCore;
using SnAbp.StdBasic.EntityFrameworkCore;
using SnAbp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SnAbp.CrPlan.EntityFrameworkCore;
using SnAbp.Material.EntityFrameworkCore;

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
            //builder.ConfigureExam();
            builder.ConfigureCms();
            builder.ConfigureStdBasic();
            builder.ConfigureBasic();
            builder.ConfigureResource();
            //builder.ConfigureEmerg();
            builder.ConfigureProblem();
            builder.ConfigureCrPlan();
            builder.ConfigureOa();
            builder.ConfigureProject();
            builder.ConfigureNotice();
            builder.ConfigureProject();
            builder.ConfigureNotice();
            builder.ConfigureMessageBpm();
            builder.ConfigureAlarm();
            builder.ConfigureTasks();
            builder.ConfigureReport();
		    builder.ConfigureSchedule();
		    builder.ConfigureMaterial();
            builder.ConfigureRegulation();
        }
    }
}
