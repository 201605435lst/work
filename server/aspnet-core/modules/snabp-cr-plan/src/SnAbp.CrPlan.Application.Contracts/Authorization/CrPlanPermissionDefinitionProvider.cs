using SnAbp.CrPlan.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.CrPlan.Authorization
{
    public class CrPlanPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            //年月表管理
            var crPlanGroup = context.AddGroup(CrPlanPermissions.CrPlanGroupName, L("Permission:CrPlan"));
            var crPlanPlanGroup = context.AddGroup(CrPlanPermissions.CrPlanPlanGroupName, L("Permission:Plan"));
            var crPlanWorkGroup = context.AddGroup(CrPlanPermissions.CrPlanWorkGroupName, L("Permission:Works"));
            var crPlanStatisticsGroup = context.AddGroup(CrPlanPermissions.CrPlanStatisticsGroupName, L("Permission:Statistics"));

            #region 年月表管理

            //年表管理
            var yearPlanPermission = crPlanGroup.AddPermission(CrPlanPermissions.YearPlan.Default, L("Permission:YearPlan"));
            yearPlanPermission.AddChild(CrPlanPermissions.YearPlan.Generate, L("Permission:Generate"));
            yearPlanPermission.AddChild(CrPlanPermissions.YearPlan.UpdateTestItems, L("Permission:UpdateTestItems"));
            yearPlanPermission.AddChild(CrPlanPermissions.YearPlan.Import, L("Permission:Import"));
            yearPlanPermission.AddChild(CrPlanPermissions.YearPlan.Export, L("Permission:Export"));
            yearPlanPermission.AddChild(CrPlanPermissions.YearPlan.Update, L("Permission:Update"));
            yearPlanPermission.AddChild(CrPlanPermissions.YearPlan.Cleared, L("Permission:Cleared"));
            yearPlanPermission.AddChild(CrPlanPermissions.YearPlan.SubmitApproval, L("Permission:SubmitApproval"));
            yearPlanPermission.AddChild(CrPlanPermissions.YearPlan.Alter, L("Permission:Alter"));
            //月表管理
            var monthPlanPermission = crPlanGroup.AddPermission(CrPlanPermissions.MonthPlan.Default, L("Permission:MonthPlan"));
            monthPlanPermission.AddChild(CrPlanPermissions.MonthPlan.Create, L("Permission:Create"));
            monthPlanPermission.AddChild(CrPlanPermissions.MonthPlan.Update, L("Permission:Update"));
            monthPlanPermission.AddChild(CrPlanPermissions.MonthPlan.Cleared, L("Permission:Cleared"));
            monthPlanPermission.AddChild(CrPlanPermissions.MonthPlan.Import, L("Permission:Import"));
            monthPlanPermission.AddChild(CrPlanPermissions.MonthPlan.Export, L("Permission:Export"));
            monthPlanPermission.AddChild(CrPlanPermissions.MonthPlan.SubmitApproval, L("Permission:SubmitApproval"));
            monthPlanPermission.AddChild(CrPlanPermissions.MonthPlan.Alter, L("Permission:Alter"));
            //年表月度计划
            var monthOfYaerPlanPermission = crPlanGroup.AddPermission(CrPlanPermissions.MonthOfYearPlan.Default, L("Permission:MonthOfYearPlan"));
            monthOfYaerPlanPermission.AddChild(CrPlanPermissions.MonthOfYearPlan.Create, L("Permission:Create"));
            monthOfYaerPlanPermission.AddChild(CrPlanPermissions.MonthOfYearPlan.Update, L("Permission:Update"));
            monthOfYaerPlanPermission.AddChild(CrPlanPermissions.MonthOfYearPlan.Import, L("Permission:Import"));
            monthOfYaerPlanPermission.AddChild(CrPlanPermissions.MonthOfYearPlan.Export, L("Permission:Export"));
            monthOfYaerPlanPermission.AddChild(CrPlanPermissions.MonthOfYearPlan.SubmitApproval, L("Permission:SubmitApproval"));
            monthOfYaerPlanPermission.AddChild(CrPlanPermissions.MonthOfYearPlan.Back, L("Permission:Back"));
            //未完成计划变更
            var planChange = crPlanGroup.AddPermission(CrPlanPermissions.PlanChange.Default, L("Permission:PlanChange"));
            planChange.AddChild(CrPlanPermissions.PlanChange.Create, L("Permission:Create"));
            planChange.AddChild(CrPlanPermissions.PlanChange.Update, L("Permission:Update"));
            planChange.AddChild(CrPlanPermissions.PlanChange.Detail, L("Permission:Detail"));
            planChange.AddChild(CrPlanPermissions.PlanChange.Delete, L("Permission:Delete"));
            planChange.AddChild(CrPlanPermissions.PlanChange.Export, L("Permission:Export"));
            planChange.AddChild(CrPlanPermissions.PlanChange.ExceedPeriod, L("Permission:ExceedPeriod"));
            #endregion

            #region 计划管理
            //维修作业
            //var maintenanceWork = crPlanPlanGroup.AddPermission(CrPlanPermissions.MaintenanceWork.Default, L("Permission:MaintenanceWork"));
            //maintenanceWork.AddChild(CrPlanPermissions.MaintenanceWork.Detail, L("Permission:Detail"));
            //maintenanceWork.AddChild(CrPlanPermissions.MaintenanceWork.Delete, L("Permission:Delete"));
            //maintenanceWork.AddChild(CrPlanPermissions.MaintenanceWork.SubmitApproval, L("Permission:SubmitApproval"));
            ////维修作业复验
            //var maintenanceWorkCheck = crPlanPlanGroup.AddPermission(CrPlanPermissions.MaintenanceWorkChcek.Default, L("Permission:MaintenanceWorkCheck"));
            //maintenanceWorkCheck.AddChild(CrPlanPermissions.MaintenanceWorkChcek.Detail, L("Permission:Detail"));
            //maintenanceWorkCheck.AddChild(CrPlanPermissions.MaintenanceWorkChcek.Update, L("Permission:Update"));
            //maintenanceWorkCheck.AddChild(CrPlanPermissions.MaintenanceWorkChcek.SubmitApproval, L("Permission:SubmitApproval"));
            //maintenanceWorkCheck.AddChild(CrPlanPermissions.MaintenanceWorkChcek.Delete, L("Permission:Delete"));


            //垂直天窗计划
            var SkylightPlan = crPlanPlanGroup.AddPermission(CrPlanPermissions.SkylightPlan.Default, L("Permission:SkylightPlan"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.Create, L("Permission:Create"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.Release, L("Permission:Release"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.Dispatching, L("Permission:Dispatching"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.Revoke, L("Permission:Revoke"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.Update, L("Permission:Update"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.Detail, L("Permission:Detail"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.Delete, L("Permission:Delete"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.AddWorkTicket, L("Permission:AddWorkTicket"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.ViewWorkFlow, L("Permission:ViewWorkFlow"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.BackReason, L("Permission:BackReason"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.DispatchingView, L("Permission:DispatchingView"));
            SkylightPlan.AddChild(CrPlanPermissions.SkylightPlan.CreateRepairPlan, L("Permission:CreateRepairPlan"));
            //综合天窗计划
            //var comprehensiveSkylightPlan = crPlanPlanGroup.AddPermission(CrPlanPermissions.ComprehensiveSkylightPlan.Default, L("Permission:ComprehensiveSkylightPlan"));
            //comprehensiveSkylightPlan.AddChild(CrPlanPermissions.ComprehensiveSkylightPlan.Create, L("Permission:Create"));
            //comprehensiveSkylightPlan.AddChild(CrPlanPermissions.ComprehensiveSkylightPlan.Release, L("Permission:Release"));
            //comprehensiveSkylightPlan.AddChild(CrPlanPermissions.ComprehensiveSkylightPlan.Dispatching, L("Permission:Dispatching"));
            //comprehensiveSkylightPlan.AddChild(CrPlanPermissions.ComprehensiveSkylightPlan.Revoke, L("Permission:Revoke"));
            //comprehensiveSkylightPlan.AddChild(CrPlanPermissions.ComprehensiveSkylightPlan.Update, L("Permission:Update"));
            //comprehensiveSkylightPlan.AddChild(CrPlanPermissions.ComprehensiveSkylightPlan.Detail, L("Permission:Detail"));
            //comprehensiveSkylightPlan.AddChild(CrPlanPermissions.ComprehensiveSkylightPlan.Delete, L("Permission:Delete"));
            ////天窗点外计划
            //var skylightOutsidePlan = crPlanPlanGroup.AddPermission(CrPlanPermissions.SkylightOutsidePlan.Default, L("Permission:SkylightOutsidePlan"));
            //skylightOutsidePlan.AddChild(CrPlanPermissions.SkylightOutsidePlan.Create, L("Permission:Create"));
            //skylightOutsidePlan.AddChild(CrPlanPermissions.SkylightOutsidePlan.Release, L("Permission:Release"));
            //skylightOutsidePlan.AddChild(CrPlanPermissions.SkylightOutsidePlan.Dispatching, L("Permission:Dispatching"));
            //skylightOutsidePlan.AddChild(CrPlanPermissions.SkylightOutsidePlan.Revoke, L("Permission:Revoke"));
            //skylightOutsidePlan.AddChild(CrPlanPermissions.SkylightOutsidePlan.Update, L("Permission:Update"));
            //skylightOutsidePlan.AddChild(CrPlanPermissions.SkylightOutsidePlan.Detail, L("Permission:Detail"));
            //skylightOutsidePlan.AddChild(CrPlanPermissions.SkylightOutsidePlan.Delete, L("Permission:Delete"));
            //其他计划
            var otherPlan = crPlanPlanGroup.AddPermission(CrPlanPermissions.OtherPlan.Default, L("Permission:OtherPlan"));
            otherPlan.AddChild(CrPlanPermissions.OtherPlan.Create, L("Permission:Create"));
            otherPlan.AddChild(CrPlanPermissions.OtherPlan.Release, L("Permission:Release"));
            otherPlan.AddChild(CrPlanPermissions.OtherPlan.Update, L("Permission:Update"));
            otherPlan.AddChild(CrPlanPermissions.OtherPlan.Detail, L("Permission:Detail"));
            otherPlan.AddChild(CrPlanPermissions.OtherPlan.Delete, L("Permission:Delete"));
            otherPlan.AddChild(CrPlanPermissions.OtherPlan.Issue, L("Permission:Issue"));
            #endregion

            #region 作业管理
            //配合作业
            var cooperateWork = crPlanWorkGroup.AddPermission(CrPlanPermissions.CooperateWork.Default, L("Permission:CooperateWork"));
            cooperateWork.AddChild(CrPlanPermissions.CooperateWork.Detail, L("Permission:Detail"));
            cooperateWork.AddChild(CrPlanPermissions.CooperateWork.Feedback, L("Permission:Feedback"));
            //派工作业
            var sendingWork = crPlanWorkGroup.AddPermission(CrPlanPermissions.SendingWork.Default, L("Permission:SendingWork"));
            sendingWork.AddChild(CrPlanPermissions.SendingWork.Detail, L("Permission:Detail"));
            sendingWork.AddChild(CrPlanPermissions.SendingWork.DispatchList, L("Permission:DispatchList"));
            sendingWork.AddChild(CrPlanPermissions.SendingWork.Finish, L("Permission:Finish"));
            sendingWork.AddChild(CrPlanPermissions.SendingWork.Acceptance, L("Permission:Acceptance"));
            sendingWork.AddChild(CrPlanPermissions.SendingWork.Revoke, L("Permission:Revoke"));
            sendingWork.AddChild(CrPlanPermissions.SendingWork.Export, L("Permission:Export"));

            //其他作业
            var otherWorks = crPlanWorkGroup.AddPermission(CrPlanPermissions.OtherWorks.Default, L("Permission:OtherWorks"));
            otherWorks.AddChild(CrPlanPermissions.OtherWorks.Detail, L("Permission:Detail"));
            otherWorks.AddChild(CrPlanPermissions.OtherWorks.Finish, L("Permission:Finish"));
            otherWorks.AddChild(CrPlanPermissions.OtherWorks.Revoke, L("Permission:Revoke"));

            //检修记录
            var maintenanceRecord = crPlanWorkGroup.AddPermission(CrPlanPermissions.MaintenanceRecord.Default, L("Permission:MaintenanceRecord"));
            maintenanceRecord.AddChild(CrPlanPermissions.MaintenanceRecord.Detail, L("Permission:Detail"));
            #endregion

            #region 智能报表

            //统计图表
            var StatisticalCharts = crPlanStatisticsGroup.AddPermission(CrPlanPermissions.StatisticalCharts.Default, L("Permission:StatisticalCharts"));
            //完成情况
            var planCompletion = crPlanStatisticsGroup.AddPermission(CrPlanPermissions.PlanCompletion.Default, L("Permission:PlanCompletion"));
            //计划追踪
            var planStateTracking = crPlanStatisticsGroup.AddPermission(CrPlanPermissions.PlanStateTracking.Default, L("Permission:PlanStateTracking"));

            #endregion
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CrPlanResource>(name);
        }
    }
}