using SnAbp.Account;
using SnAbp.Basic;
using SnAbp.Bpm;
using SnAbp.Cms;
using SnAbp.Common;
//using SnAbp.CrPlan;

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
using SnAbp.Material;
using SnAbp.Technology;
using SnAbp.CostManagement;
using SnAbp.Tasks;
using SnAbp.Emerg;
using SnAbp.Safe;
using SnAbp.Quality;
using SnAbp.ConstructionBase;
using SnAbp.CrPlan;
using SnAbp.Construction;
using SnAbp.FileApprove;

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
        typeof(CmsApplicationModule),
        typeof(FileApplicationModule),

        typeof(StdBasicApplicationModule),
        typeof(BasicApplicationModule),
        typeof(ResourceApplicationModule),
        typeof(EmergApplicationModule),
        typeof(ProblemApplicationModule),
        typeof(CommonApplicationModule),
        typeof(OaApplicationModule),
        typeof(CrPlanApplicationModule),
        typeof(AlarmApplicationModule),
        typeof(ProjectApplicationModule),
        typeof(ReportApplicationModule),
        typeof(RegulationApplicationModule),
        typeof(MaterialApplicationModule),
        typeof(TechnologyApplicationModule),
        typeof(CostManagementApplicationModule),
        typeof(SafeApplicationModule),
        typeof(QualityApplicationModule),
        typeof(ConstructionBaseApplicationModule),
        typeof(ConstructionApplicationModule),
        typeof(TasksApplicationModule),
        typeof(FileApproveApplicationModule)
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
