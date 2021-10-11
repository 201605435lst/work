using SnAbp.Construction.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Construction.Permissions
{
    public class ConstructionPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(ConstructionPermissions.GroupName, L("Permission:Construction"));

            // 定义派工单模板管理的权限
            PermissionDefinition dispatchTemplatePermission = myGroup.AddPermission(ConstructionPermissions.DispatchTemplatePermission.Default, L("Permission:DispatchTemplate"));
            dispatchTemplatePermission.AddChild(ConstructionPermissions.DispatchTemplatePermission.Create, L("Permission:Create"));
            dispatchTemplatePermission.AddChild(ConstructionPermissions.DispatchTemplatePermission.Update, L("Permission:Update"));
            dispatchTemplatePermission.AddChild(ConstructionPermissions.DispatchTemplatePermission.Detail, L("Permission:Detail"));
            dispatchTemplatePermission.AddChild(ConstructionPermissions.DispatchTemplatePermission.Delete, L("Permission:Delete"));
            // 定义施工日志模板管理的权限
            PermissionDefinition dailyTemplatesPermission = myGroup.AddPermission(ConstructionPermissions.DailyTemplates.Default, L("Permission:DailyTemplates"));
            dailyTemplatesPermission.AddChild(ConstructionPermissions.DailyTemplates.Create, L("Permission:Create"));
            dailyTemplatesPermission.AddChild(ConstructionPermissions.DailyTemplates.Update, L("Permission:Update"));
            dailyTemplatesPermission.AddChild(ConstructionPermissions.DailyTemplates.Detail, L("Permission:Detail"));
            dailyTemplatesPermission.AddChild(ConstructionPermissions.DailyTemplates.Delete, L("Permission:Delete"));
            dailyTemplatesPermission.AddChild(ConstructionPermissions.DailyTemplates.UpdateDefault, L("Permission:UpdateDefault"));

            // 定义派工单管理的权限
            PermissionDefinition dispatchPermission = myGroup.AddPermission(ConstructionPermissions.DispatchPermission.Default, L("Permission:Dispatch"));
            dispatchPermission.AddChild(ConstructionPermissions.DispatchPermission.Create, L("Permission:Create"));
            dispatchPermission.AddChild(ConstructionPermissions.DispatchPermission.Update, L("Permission:Update"));
            dispatchPermission.AddChild(ConstructionPermissions.DispatchPermission.Detail, L("Permission:Detail"));
            dispatchPermission.AddChild(ConstructionPermissions.DispatchPermission.Delete, L("Permission:Delete"));
            dispatchPermission.AddChild(ConstructionPermissions.DispatchPermission.Export, L("Permission:Export"));
            dispatchPermission.AddChild(ConstructionPermissions.DispatchPermission.Submit, L("Permission:Submit"));
            dispatchPermission.AddChild(ConstructionPermissions.DispatchPermission.Approval, L("Permission:Approval"));

            // 定义施工计划工程量的权限
            PermissionDefinition planMaterialPermission = myGroup.AddPermission(ConstructionPermissions.PlanMaterialPermission.Default, L("Permission:PlanMaterial"));
            planMaterialPermission.AddChild(ConstructionPermissions.PlanMaterialPermission.Create, L("Permission:Create"));
            planMaterialPermission.AddChild(ConstructionPermissions.PlanMaterialPermission.Update, L("Permission:Update"));
            planMaterialPermission.AddChild(ConstructionPermissions.PlanMaterialPermission.Detail, L("Permission:Detail"));
            planMaterialPermission.AddChild(ConstructionPermissions.PlanMaterialPermission.Delete, L("Permission:Delete"));


            // 定义施工计划详情的权限
            PermissionDefinition planContentPermission = myGroup.AddPermission(ConstructionPermissions.PlanContentPermission.Default, L("Permission:PlanContent"));
            planContentPermission.AddChild(ConstructionPermissions.PlanContentPermission.Create, L("Permission:Create"));
            planContentPermission.AddChild(ConstructionPermissions.PlanContentPermission.Update, L("Permission:Update"));
            planContentPermission.AddChild(ConstructionPermissions.PlanContentPermission.Detail, L("Permission:Detail"));
            planContentPermission.AddChild(ConstructionPermissions.PlanContentPermission.Delete, L("Permission:Delete"));


            // 定义施工计划的权限
            PermissionDefinition planPermission = myGroup.AddPermission(ConstructionPermissions.PlanPermission.Default, L("Permission:Plan"));
            planPermission.AddChild(ConstructionPermissions.PlanPermission.Create, L("Permission:Create"));
            planPermission.AddChild(ConstructionPermissions.PlanPermission.Update, L("Permission:Update"));
            planPermission.AddChild(ConstructionPermissions.PlanPermission.Detail, L("Permission:Detail"));
            planPermission.AddChild(ConstructionPermissions.PlanPermission.Delete, L("Permission:Delete"));
            planPermission.AddChild(ConstructionPermissions.PlanPermission.View, L("Permission:View"));
            planPermission.AddChild(ConstructionPermissions.PlanPermission.Draw, L("Permission:Draw"));
            planPermission.AddChild(ConstructionPermissions.PlanPermission.Approve, L("Permission:Approve"));


            // 定义总体计划详情的权限
            PermissionDefinition masterPlanContentPermission = myGroup.AddPermission(ConstructionPermissions.MasterPlanContentPermission.Default, L("Permission:MasterPlanContent"));
            masterPlanContentPermission.AddChild(ConstructionPermissions.MasterPlanContentPermission.Create, L("Permission:Create"));
            masterPlanContentPermission.AddChild(ConstructionPermissions.MasterPlanContentPermission.Update, L("Permission:Update"));
            masterPlanContentPermission.AddChild(ConstructionPermissions.MasterPlanContentPermission.Detail, L("Permission:Detail"));
            masterPlanContentPermission.AddChild(ConstructionPermissions.MasterPlanContentPermission.Delete, L("Permission:Delete"));
            // 定义施工日志权限
            PermissionDefinition dailyPermission = myGroup.AddPermission(ConstructionPermissions.Dailys.Default, L("Permission:Dailys"));
            dailyPermission.AddChild(ConstructionPermissions.Dailys.Create, L("Permission:Create"));
            dailyPermission.AddChild(ConstructionPermissions.Dailys.Update, L("Permission:Update"));
            dailyPermission.AddChild(ConstructionPermissions.Dailys.Detail, L("Permission:Detail"));
            dailyPermission.AddChild(ConstructionPermissions.Dailys.Delete, L("Permission:Delete"));
            dailyPermission.AddChild(ConstructionPermissions.Dailys.Submit, L("Permission:Submit"));
            dailyPermission.AddChild(ConstructionPermissions.Dailys.Approve, L("Permission:Approve"));
            dailyPermission.AddChild(ConstructionPermissions.Dailys.ApproveFlow, L("Permission:ApproveFlow"));


            // 定义总体计划详情的权限
            PermissionDefinition masterPlanPermission = myGroup.AddPermission(ConstructionPermissions.MasterPlanPermission.Default, L("Permission:MasterPlan"));
            masterPlanPermission.AddChild(ConstructionPermissions.MasterPlanPermission.Create, L("Permission:Create"));
            masterPlanPermission.AddChild(ConstructionPermissions.MasterPlanPermission.Update, L("Permission:Update"));
            masterPlanPermission.AddChild(ConstructionPermissions.MasterPlanPermission.Detail, L("Permission:Detail"));
            masterPlanPermission.AddChild(ConstructionPermissions.MasterPlanPermission.Delete, L("Permission:Delete"));
            masterPlanPermission.AddChild(ConstructionPermissions.MasterPlanPermission.View, L("Permission:View"));
            masterPlanPermission.AddChild(ConstructionPermissions.MasterPlanPermission.Draw, L("Permission:Draw"));
            masterPlanPermission.AddChild(ConstructionPermissions.MasterPlanPermission.Approve, L("Permission:Approve"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ConstructionResource>(name);
        }
    }
}
