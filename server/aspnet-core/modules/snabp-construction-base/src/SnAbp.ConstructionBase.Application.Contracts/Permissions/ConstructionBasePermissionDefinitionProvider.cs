using SnAbp.ConstructionBase.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.ConstructionBase.Permissions
{
    public class ConstructionBasePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            PermissionGroupDefinition moduleGroup = context.AddGroup(ConstructionBasePermissions.GroupName, L(ConstructionBasePermissions.GroupName));

			// 定义施工区段的权限
            PermissionDefinition sectionPermission= moduleGroup.AddPermission(ConstructionBasePermissions.Section.Default, L("Permission:Section"));
            sectionPermission.AddChild(ConstructionBasePermissions.Section.Create, L("Permission:Create"));
            sectionPermission.AddChild(ConstructionBasePermissions.Section.Update, L("Permission:Update"));
            sectionPermission.AddChild(ConstructionBasePermissions.Section.Detail, L("Permission:Detail"));
            sectionPermission.AddChild(ConstructionBasePermissions.Section.Delete, L("Permission:Delete"));


			// 定义工序规范维护的权限
            PermissionDefinition standardPermission= moduleGroup.AddPermission(ConstructionBasePermissions.Standard.Default, L("Permission:Standard"));
            standardPermission.AddChild(ConstructionBasePermissions.Standard.Create, L("Permission:Create"));
            standardPermission.AddChild(ConstructionBasePermissions.Standard.Update, L("Permission:Update"));
            standardPermission.AddChild(ConstructionBasePermissions.Standard.Detail, L("Permission:Detail"));
            standardPermission.AddChild(ConstructionBasePermissions.Standard.Delete, L("Permission:Delete"));

            // 先定义 组的权限 ,后面 按钮 的权限 在说……
            PermissionDefinition workerPermission        = moduleGroup.AddPermission(ConstructionBasePermissions.Worker.Default        , L("Permission:Worker"));
            workerPermission.AddChild(ConstructionBasePermissions.Worker.Create, L("Permission:Create"));
            workerPermission.AddChild(ConstructionBasePermissions.Worker.Update, L("Permission:Update"));
            workerPermission.AddChild(ConstructionBasePermissions.Worker.Detail, L("Permission:Detail"));
            workerPermission.AddChild(ConstructionBasePermissions.Worker.Delete, L("Permission:Delete"));
            PermissionDefinition equipmentTeamPermission = moduleGroup.AddPermission(ConstructionBasePermissions.EquipmentTeam.Default , L("Permission:EquipmentTeam"));
            equipmentTeamPermission.AddChild(ConstructionBasePermissions.EquipmentTeam.Create, L("Permission:Create"));
            equipmentTeamPermission.AddChild(ConstructionBasePermissions.EquipmentTeam.Update, L("Permission:Update"));
            equipmentTeamPermission.AddChild(ConstructionBasePermissions.EquipmentTeam.Detail, L("Permission:Detail"));
            equipmentTeamPermission.AddChild(ConstructionBasePermissions.EquipmentTeam.Delete, L("Permission:Delete"));
            
            PermissionDefinition materialPermission      = moduleGroup.AddPermission(ConstructionBasePermissions.Material.Default      , L("Permission:Material"));
            materialPermission.AddChild(ConstructionBasePermissions.Material.Create, L("Permission:Create"));
            materialPermission.AddChild(ConstructionBasePermissions.Material.Update, L("Permission:Update"));
            materialPermission.AddChild(ConstructionBasePermissions.Material.Detail, L("Permission:Detail"));
            materialPermission.AddChild(ConstructionBasePermissions.Material.Delete, L("Permission:Delete"));
            PermissionDefinition procedurePermission     = moduleGroup.AddPermission(ConstructionBasePermissions.Procedure.Default     , L("Permission:Procedure"));
            procedurePermission.AddChild(ConstructionBasePermissions.Procedure.Create, L("Permission:Create"));
            procedurePermission.AddChild(ConstructionBasePermissions.Procedure.Update, L("Permission:Update"));
            procedurePermission.AddChild(ConstructionBasePermissions.Procedure.Detail, L("Permission:Detail"));
            procedurePermission.AddChild(ConstructionBasePermissions.Procedure.Delete, L("Permission:Delete"));
            PermissionDefinition subItemPermission       = moduleGroup.AddPermission(ConstructionBasePermissions.SubItem.Default       , L("Permission:SubItem"));
            subItemPermission.AddChild(ConstructionBasePermissions.SubItem.Create, L("Permission:Create"));
            subItemPermission.AddChild(ConstructionBasePermissions.SubItem.Update, L("Permission:Update"));
            subItemPermission.AddChild(ConstructionBasePermissions.SubItem.Detail, L("Permission:Detail"));
            subItemPermission.AddChild(ConstructionBasePermissions.SubItem.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ConstructionBaseResource>(name);
        }
    }
}
