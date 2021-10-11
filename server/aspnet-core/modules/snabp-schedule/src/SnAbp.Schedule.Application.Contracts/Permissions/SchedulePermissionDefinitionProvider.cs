using SnAbp.Schedule.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Schedule.Permissions
{
    public class SchedulePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(SchedulePermissions.GroupName, L(SchedulePermissions.GroupName));

            var schedulePerm = moduleGroup.AddPermission(SchedulePermissions.Schedule.Default, L("Permission:Schedule"));
            schedulePerm.AddChild(SchedulePermissions.Schedule.Create, L("Permission:Create"));
            schedulePerm.AddChild(SchedulePermissions.Schedule.Delete, L("Permission:Delete"));
            schedulePerm.AddChild(SchedulePermissions.Schedule.Update, L("Permission:Update"));
            schedulePerm.AddChild(SchedulePermissions.Schedule.Detail, L("Permission:Detail"));
            schedulePerm.AddChild(SchedulePermissions.Schedule.Import, L("Permission:Import"));
            schedulePerm.AddChild(SchedulePermissions.Schedule.Export, L("Permission:Export"));
            schedulePerm.AddChild(SchedulePermissions.Schedule.SetFlow, L("Permission:SetFlow"));

            var approvalPerm = moduleGroup.AddPermission(SchedulePermissions.Approval.Default, L("Permission:Approval"));
            approvalPerm.AddChild(SchedulePermissions.Approval.Create, L("Permission:Create"));
            approvalPerm.AddChild(SchedulePermissions.Approval.Delete, L("Permission:Delete"));
            approvalPerm.AddChild(SchedulePermissions.Approval.Update, L("Permission:Update"));
            approvalPerm.AddChild(SchedulePermissions.Approval.Detail, L("Permission:Detail"));
            approvalPerm.AddChild(SchedulePermissions.Approval.Export, L("Permission:Export"));
            var diaryPerm = moduleGroup.AddPermission(SchedulePermissions.Diary.Default, L("Permission:Diary"));
            diaryPerm.AddChild(SchedulePermissions.Diary.Fill, L("Permission:Fill"));
            diaryPerm.AddChild(SchedulePermissions.Diary.View, L("Permission:View"));
            diaryPerm.AddChild(SchedulePermissions.Diary.Update, L("Permission:Update"));
            diaryPerm.AddChild(SchedulePermissions.Diary.PdfExport, L("Permission:PdfExport"));
            diaryPerm.AddChild(SchedulePermissions.Diary.ExcelExport, L("Permission:ExcelExport"));
            diaryPerm.AddChild(SchedulePermissions.Diary.LogStatistics, L("Permission:LogStatistics"));
            diaryPerm.AddChild(SchedulePermissions.Diary.Examination, L("Permission:Examination"));
        }
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ScheduleResource>(name);
        }
    }
}