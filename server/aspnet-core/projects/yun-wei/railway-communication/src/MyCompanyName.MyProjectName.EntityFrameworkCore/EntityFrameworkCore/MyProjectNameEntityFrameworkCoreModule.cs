using Microsoft.Extensions.DependencyInjection;
using SnAbp.Basic.EntityFrameworkCore;
using SnAbp.Bpm.EntityFrameworkCore;
using SnAbp.Cms.EntityFrameworkCore;
using SnAbp.Common.EntityFrameworkCore;
using SnAbp.CrPlan.EntityFrameworkCore;
//using SnAbp.Emerg.EntityFrameworkCore;
//using SnAbp.Exam.EntityFrameworkCore;
using SnAbp.FeatureManagement.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.IdentityServer.EntityFrameworkCore;
using SnAbp.Message.Notice.EntityFrameworkCore;
using SnAbp.Oa.EntityFrameworkCore;
using SnAbp.PermissionManagement.EntityFrameworkCore;
using SnAbp.Problem.EntityFrameworkCore;
using SnAbp.Project.EntityFrameworkCore;
using SnAbp.Alarm.EntityFrameworkCore;
using SnAbp.Message.Bpm.EntityFrameworkCore;
using SnAbp.Regulation.EntityFrameworkCore;
using SnAbp.Resource.EntityFrameworkCore;
using SnAbp.SettingManagement.EntityFrameworkCore;
using SnAbp.StdBasic.EntityFrameworkCore;
using SnAbp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;
using SnAbp.Tasks.EntityFrameworkCore;
using SnAbp.Schedule.EntityFrameworkCore;
using SnAbp.Material.EntityFrameworkCore;
using SnAbp.Report.EntityFrameworkCore;

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
        //typeof(ExamEntityFrameworkCoreModule),
        typeof(CmsEntityFrameworkCoreModule),
        typeof(FileEntityFrameworkCoreModule),

        typeof(StdBasicEntityFrameworkCoreModule),
        typeof(BasicEntityFrameworkCoreModule),
        typeof(ResourceEntityFrameworkCoreModule),
        //typeof(EmergEntityFrameworkCoreModule),

        typeof(ProblemEntityFrameworkCoreModule),
        typeof(OaEntityFrameworkCoreModule),
        typeof(NoticeEntityFrameworkCoreModule),
        typeof(ProjectEntityFrameworkCoreModule),
        typeof(CrPlanEntityFrameworkCoreModule),
        typeof(OaEntityFrameworkCoreModule),
        typeof(NoticeEntityFrameworkCoreModule),
        typeof(BpmMessageEntityFrameworkCoreModule),
        typeof(ProjectEntityFrameworkCoreModule),
        typeof(AlarmEntityFrameworkCoreModule),
        typeof(CrPlanEntityFrameworkCoreModule),
        typeof(ReportEntityFrameworkCoreModule),
        typeof(RegulationEntityFrameworkCoreModule),
        typeof(ScheduleEntityFrameworkCoreModule),
        typeof(MaterialEntityFrameworkCoreModule),
        typeof(TasksEntityFrameworkCoreModule)
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
