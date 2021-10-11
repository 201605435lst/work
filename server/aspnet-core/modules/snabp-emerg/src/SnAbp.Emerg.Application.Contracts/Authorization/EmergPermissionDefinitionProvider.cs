using SnAbp.Emerg.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Emerg.Authorization
{
    public class EmergPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(EmergPermissions.GroupName, L(EmergPermissions.GroupName));


            var faultPerm = moduleGroup.AddPermission(EmergPermissions.Fault.Default, L("Permission:Faults"));
            faultPerm.AddChild(EmergPermissions.Fault.Create, L("Permission:Create"));
            faultPerm.AddChild(EmergPermissions.Fault.Delete, L("Permission:Delete"));
            faultPerm.AddChild(EmergPermissions.Fault.Update, L("Permission:Update"));
            faultPerm.AddChild(EmergPermissions.Fault.Detail, L("Permission:Detail"));
           

            var planPerm = moduleGroup.AddPermission(EmergPermissions.Plan.Default, L("Permission:Plans"));
            planPerm.AddChild(EmergPermissions.Plan.Create, L("Permission:Create"));
            planPerm.AddChild(EmergPermissions.Plan.Delete, L("Permission:Delete"));
            planPerm.AddChild(EmergPermissions.Plan.Update, L("Permission:Update"));
            planPerm.AddChild(EmergPermissions.Plan.Detail, L("Permission:Detail"));
            faultPerm.AddChild(EmergPermissions.Plan.Apply, L("Permission:Apply"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<EmergResource>(name);
        }
    }
}