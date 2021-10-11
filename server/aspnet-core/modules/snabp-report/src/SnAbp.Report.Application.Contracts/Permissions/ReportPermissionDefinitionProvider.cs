using SnAbp.Report.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Report.Permissions
{
    public class ReportPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(ReportPermissions.GroupName, L(ReportPermissions.GroupName));
            var reportPerm = moduleGroup.AddPermission(ReportPermissions.Report.Default, L("Permission:Reports"));
            reportPerm.AddChild(ReportPermissions.Report.Create, L("Permission:Create"));
            reportPerm.AddChild(ReportPermissions.Report.Delete, L("Permission:Delete"));
            reportPerm.AddChild(ReportPermissions.Report.Update, L("Permission:Update"));
            reportPerm.AddChild(ReportPermissions.Report.Detail, L("Permission:Detail"));
            reportPerm.AddChild(ReportPermissions.Report.Export, L("Permission:Export"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ReportResource>(name);
        }
    }
}