using SnAbp.Oa.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Oa.Permissions
{
    public class OaPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {

            var moduleGroup = context.AddGroup(OaPermissions.GroupName, L(OaPermissions.GroupName));

            var dutySchedulePerm = moduleGroup.AddPermission(OaPermissions.DutySchedule.Default, L("Permission:DutyScheduleManagement"));
            dutySchedulePerm.AddChild(OaPermissions.DutySchedule.Create, L("Permission:Create"));
            dutySchedulePerm.AddChild(OaPermissions.DutySchedule.Delete, L("Permission:Delete"));
            dutySchedulePerm.AddChild(OaPermissions.DutySchedule.Update, L("Permission:Update"));
            dutySchedulePerm.AddChild(OaPermissions.DutySchedule.Detail, L("Permission:Detail"));

            var contractPerm = moduleGroup.AddPermission(OaPermissions.Contract.Default, L("Permission:Contracts"));
            contractPerm.AddChild(OaPermissions.Contract.Create, L("Permission:Create"));
            contractPerm.AddChild(OaPermissions.Contract.Delete, L("Permission:Delete"));
            contractPerm.AddChild(OaPermissions.Contract.Update, L("Permission:Update"));
            contractPerm.AddChild(OaPermissions.Contract.Detail, L("Permission:Detail"));
            contractPerm.AddChild(OaPermissions.Contract.Apply, L("Permission:Apply"));
            contractPerm.AddChild(OaPermissions.Contract.Export, L("Permission:Export"));
            //dutySchedulePerm.AddChild(OaPermissions.DutySchedule.ManagePermissions, L("Permission:ChangePermissions"));

            var SealPerm = moduleGroup.AddPermission(OaPermissions.Seal.Default, L("Permission:Seals"));
            SealPerm.AddChild(OaPermissions.Seal.Create, L("Permission:Create"));
            SealPerm.AddChild(OaPermissions.Seal.Delete, L("Permission:Delete"));
            SealPerm.AddChild(OaPermissions.Seal.Lock, L("Permission:Lock"));
            SealPerm.AddChild(OaPermissions.Seal.Efficiency, L("Permission:Efficiency"));
            SealPerm.AddChild(OaPermissions.Seal.RestPSW, L("Permission:RestPSW"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<OaResource>(name);
        }
    }
}