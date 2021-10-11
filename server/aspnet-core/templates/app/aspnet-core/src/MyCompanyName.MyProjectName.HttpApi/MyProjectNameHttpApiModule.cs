using SnAbp.Account;
using SnAbp.Bpm;
using SnAbp.Cms;

using SnAbp.Common;

using SnAbp.FeatureManagement;
using SnAbp.File;
using SnAbp.Identity;
using SnAbp.Message.Notice;
using SnAbp.Oa;
using SnAbp.PermissionManagement.HttpApi;
using SnAbp.Problem;
using SnAbp.Resource;
using SnAbp.StdBasic;
using SnAbp.Project;
using SnAbp.Alarm;
using SnAbp.Tasks;
using SnAbp.TenantManagement;
using Volo.Abp.Modularity;
using SnAbp.Message.Bpm;
using SnAbp.Report;
using SnAbp.Material;
using SnAbp.Technology;
using SnAbp.CostManagement;
using SnAbp.Regulation;
using SnAbp.Emerg;
using SnAbp.CrPlan;
using SnAbp.Safe;
using SnAbp.ConstructionBase;
using SnAbp.Construction;
using SnAbp.Quality;
using SnAbp.FileApprove;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationContractsModule),
        typeof(SnAbpAccountHttpApiModule),
        typeof(SnAbpIdentityHttpApiModule),
        typeof(SnAbpPermissionManagementHttpApiModule),
        typeof(SnAbpTenantManagementHttpApiModule),
        typeof(SnAbpFeatureManagementHttpApiModule),
        typeof(NoticeHttpApiModule),
        typeof(MessageBpmHttpApiModule),
        typeof(BpmHttpApiModule),
        typeof(CmsHttpApiModule),
        typeof(FileHttpApiModule),
        typeof(StdBasicHttpApiModule),
        typeof(StdBasicHttpApiModule),
        typeof(ResourceHttpApiModule),
        typeof(EmergHttpApiModule),
        typeof(ProblemHttpApiModule),
        typeof(CommonHttpApiModule),
        typeof(CrPlanHttpApiModule),
        typeof(ProjectHttpApiModule),
        typeof(ProblemHttpApiModule),
        typeof(OaHttpApiModule),
        typeof(TasksHttpApiModule),
        typeof(RegulationHttpApiModule),
        typeof(MaterialHttpApiModule),
        typeof(TechnologyHttpApiModule),
        typeof(CostManagementHttpApiModule),
        typeof(ReportHttpApiModule),
        typeof(AlarmHttpApiModule),
        typeof(ConstructionBaseHttpApiModule),
		typeof(ConstructionHttpApiModule),
        typeof(SafeHttpApiModule),
        typeof(QualityHttpApiModule),
        typeof(FileApproveHttpApiModule)

        )]
    public class MyProjectNameHttpApiModule : AbpModule
    {
    }
}
