using Volo.Abp.Reflection;

namespace SnAbp.CrPlan.Authorization
{
    public class CrPlanPermissions
    {
        public const string CrPlanGroupName = "AbpCrPlan";                      //���±�����
        public const string CrPlanPlanGroupName = "AbpCrPlanPlan";              //�ƻ�����
        public const string CrPlanWorkGroupName = "AbpCrPlanWork";              //��ҵ����
        public const string CrPlanStatisticsGroupName = "AbpCrPlanStatistics";  //���ܱ���

        #region ���±�����

        public static class YearPlan
        {
            public const string Default = CrPlanGroupName + ".YearPlan";
            public const string Generate = Default + ".Generate";
            public const string UpdateTestItems = Default + ".UpdateTestItems";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
            public const string SubmitApproval = Default + ".SubmitApproval";
            public const string Alter = Default + ".Alter";
            public const string Update = Default + ".Update";
            public const string Cleared = Default + ".Cleared";

        }

        public static class MonthPlan
        {
            public const string Default = CrPlanGroupName + ".MonthPlan";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
            public const string SubmitApproval = Default + ".SubmitApproval";
            public const string Alter = Default + ".Alter";
            public const string Cleared = Default + ".Cleared";
        }

        public static class MonthOfYearPlan
        {
            public const string Default = CrPlanGroupName + ".MonthOfYearPlan";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
            public const string SubmitApproval = Default + ".SubmitApproval";
            public const string Back = Default + ".Back";
        }

        public static class PlanChange
        {
            public const string Default = CrPlanGroupName + ".PlanChange";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Export = Default + ".Export";
            public const string ExceedPeriod = Default + ".ExceedPeriod";
        }

        #endregion

        #region �ƻ�����

        public static class MaintenanceWork
        {
            public const string Default = CrPlanPlanGroupName + ".MaintenanceWork";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete"; 
            public const string SubmitApproval = Default + ".SubmitApproval";
        }
        public static class MaintenanceWorkChcek
        {
            public const string Default = CrPlanPlanGroupName + ".MaintenanceWorkCheck";
            public const string Detail = Default + ".Detail";
            public const string Update = Default + ".Update";
            public const string SubmitApproval = Default + ".SubmitApproval";
            public const string Delete = Default + ".Delete";
        }
        public static class SkylightPlan
        {
            public const string Default = CrPlanPlanGroupName + ".SkylightPlan";
            public const string Create = Default + ".Create";
            public const string Release = Default + ".Release";
            public const string Dispatching = Default + ".Dispatching";
            public const string Revoke = Default + ".Revoke";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string CreateRepairPlan = Default + ".CreateRepairPlan";
            public const string AddWorkTicket = Default + ".AddWorkTicket";
            public const string ViewWorkFlow = Default + ".ViewWorkFlow";
            public const string BackReason = Default + ".BackReason";
            public const string DispatchingView = Default + ".DispatchingView";
        }

        //public static class ComprehensiveSkylightPlan
        //{
        //    public const string Default = CrPlanPlanGroupName + ".ComprehensiveSkylightPlan";
        //    public const string Create = Default + ".Create";
        //    public const string Release = Default + ".Release";
        //    public const string Dispatching = Default + ".Dispatching";
        //    public const string Revoke = Default + ".Revoke";
        //    public const string Update = Default + ".Update";
        //    public const string Detail = Default + ".Detail";
        //    public const string Delete = Default + ".Delete";
        //}

        //public static class SkylightOutsidePlan
        //{
        //    public const string Default = CrPlanPlanGroupName + ".SkylightOutsidePlan";
        //    public const string Create = Default + ".Create";
        //    public const string Release = Default + ".Release";
        //    public const string Dispatching = Default + ".Dispatching";
        //    public const string Revoke = Default + ".Revoke";
        //    public const string Update = Default + ".Update";
        //    public const string Detail = Default + ".Detail";
        //    public const string Delete = Default + ".Delete";
        //}

        public static class OtherPlan
        {
            public const string Default = CrPlanPlanGroupName + ".OtherPlan";
            public const string Create = Default + ".Create";
            public const string Release = Default + ".Release";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Issue = Default + ".Issue";
        }

        #endregion

        #region ��ҵ����

        public static class SendingWork
        {
            public const string Default = CrPlanWorkGroupName + ".SendingWork";
            public const string Detail = Default + ".Detail";
            public const string DispatchList = Default + ".DispatchList";
            public const string Finish = Default + ".Finish";
            public const string Acceptance = Default + ".Acceptance";
            public const string Revoke = Default + ".Revoke";
            public const string Export = Default + ".Export";
        }

        public static class OtherWorks
        {
            public const string Default = CrPlanWorkGroupName + ".OtherWorks";
            public const string Detail = Default + ".Detail";
            public const string Finish = Default + ".Finish";
            public const string Revoke = Default + ".Revoke";
        }

        public static class MaintenanceRecord
        {
            public const string Default = CrPlanWorkGroupName + ".MaintenanceRecord";
            public const string Detail = Default + ".Detail";
        }

        //配合作业
        public static class CooperateWork
        {
            public const string Default = CrPlanWorkGroupName + ".CooperateWork";
            public const string Detail = Default + ".Detail";
            public const string Feedback = Default + ".Feedback";
        }
        #endregion

        #region ���ܱ���

        public static class StatisticalCharts
        {
            public const string Default = CrPlanStatisticsGroupName + ".StatisticalCharts";
        }

        public static class PlanCompletion
        {
            public const string Default = CrPlanStatisticsGroupName + ".PlanCompletion";
        }

        public static class PlanStateTracking
        {
            public const string Default = CrPlanStatisticsGroupName + ".PlanStateTracking";
        }
        #endregion

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(CrPlanPermissions));
        }
    }
}