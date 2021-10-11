using SnAbp.FileApprove.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.FileApprove.Permissions
{
    public class FileApprovePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(FileApprovePermissions.GroupName, L("Permission:FileApprove"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FileApproveResource>(name);
        }
    }
}