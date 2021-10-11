using Volo.Abp.Reflection;

namespace SnAbp.Construction.Permissions
{
    public class ConstructionPermissions
    {
        public const string GroupName = "Construction";


        public static class MasterPlanPermission
        {
            public const string Default = GroupName + ".MasterPlan";

            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View"; // 查看 
            public const string Draw = Default + ".Draw"; // 编制 
            public const string Approve = Default + ".Approve"; // 审批 
        }
        public static class Dailys
        {
            public const string Default = GroupName + ".Dailys";

            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Submit = Default + ".Submit"; // 提交 
            public const string Approve = Default + ".Approve"; // 审批 
            public const string ApproveFlow = Default + ".ApproveFlow"; // 查看流程 

        }

        public static class MasterPlanContentPermission
        {
            public const string Default = GroupName + ".MasterPlanContent";

            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }


        public static class PlanPermission
        {
            public const string Default = GroupName + ".Plan";

            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View"; // 查看 
            public const string Draw = Default + ".Draw"; // 编制 
            public const string Approve = Default + ".Approve"; // 审批 
        }


        public static class PlanContentPermission
        {
            public const string Default = GroupName + ".PlanContent";

            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }


        public static class PlanMaterialPermission
        {
            public const string Default = GroupName + ".PlanMaterial";

            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }


        public static class DispatchTemplatePermission
        {
            public const string Default = GroupName + ".DispatchTemplate";

            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }
        public static class DailyTemplates
        {
            public const string Default = GroupName + ".DailyTemplates";

            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string UpdateDefault = Default + ".UpdateDefault";
        }
        public static class DispatchPermission
        {
            public const string Default = GroupName + ".Dispatch";

            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Export = Default + ".Export";
            public const string Submit = Default + ".Submit"; // 提交审批 
            public const string Approval = Default + ".Approval"; // 审批 
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ConstructionPermissions));
        }
    }
}
