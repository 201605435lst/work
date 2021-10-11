using SnAbp.Account;
using SnAbp.Basic;
using SnAbp.Bpm;
using SnAbp.Cms;
using SnAbp.Common;
using SnAbp.CrPlan;
//using SnAbp.Emerg;
//using SnAbp.Exam;
using SnAbp.FeatureManagement;
using SnAbp.File;
using SnAbp.Identity;
using SnAbp.Message;
using SnAbp.Oa;
using SnAbp.PermissionManagement;
using SnAbp.Problem;
using SnAbp.Resource;
using SnAbp.StdBasic;
using SnAbp.Project;
using SnAbp.TenantManagement;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using SnAbp.Alarm;
using SnAbp.Regulation;
using SnAbp.Report;
using SnAbp.Schedule;
using SnAbp.Material;
using SnAbp.Tasks;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(SnAbpAccountApplicationModule),
        typeof(MyProjectNameApplicationContractsModule),
        typeof(SnAbpIdentityApplicationModule),
        typeof(SnAbpPermissionManagementApplicationModule),
        typeof(SnAbpTenantManagementApplicationModule),
        typeof(SnAbpFeatureManagementApplicationModule),
        typeof(BpmApplicationModule),
        //typeof(ExamApplicationModule),
        typeof(CmsApplicationModule),
        typeof(FileApplicationModule),

        typeof(StdBasicApplicationModule),
        typeof(BasicApplicationModule),
        typeof(ResourceApplicationModule),
        //typeof(EmergApplicationModule),
        typeof(ProblemApplicationModule),
        typeof(CommonApplicationModule),
        typeof(OaApplicationModule),
        typeof(CrPlanApplicationModule),
        typeof(AlarmApplicationModule),
        typeof(ProjectApplicationModule),
        typeof(ReportApplicationModule),
        typeof(RegulationApplicationModule),
        typeof(ScheduleApplicationModule),
        typeof(MaterialApplicationModule),
        typeof(TasksApplicationModule)
        )]
    public class MyProjectNameApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<MyProjectNameApplicationModule>();
            });
        }
    }
}
