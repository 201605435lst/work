
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.File.Authorization
{
    public class FilePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(FilePermissions.GroupName, L(FilePermissions.GroupName));


            //var filePerm = moduleGroup.AddPermission(FilePermissions.File.Default, L("Permission:FileManagement"));
            //filePerm.AddChild(FilePermissions.File.Create, L("Permission:Create"));
            //filePerm.AddChild(FilePermissions.File.Delete, L("Permission:Delete"));
            //filePerm.AddChild(FilePermissions.File.Update, L("Permission:Update"));
            //filePerm.AddChild(FilePermissions.File.ManagePermissions, L("Permission:ChangePermissions"));

            var fileManagerPerm = moduleGroup.AddPermission(FilePermissions.FileManager.Default, L("Permission:FileManager"));
            //fileManagerPerm.AddChild(FilePermissions.FileManager.Create, L("Permission:Create"));
            //fileManagerPerm.AddChild(FilePermissions.FileManager.Delete, L("Permission:Delete"));
            //fileManagerPerm.AddChild(FilePermissions.FileManager.Update, L("Permission:Update"));
            //fileManagerPerm.AddChild(FilePermissions.FileManager.ManagePermissions, L("Permission:ChangePermissions"));

            //var folderPerm = moduleGroup.AddPermission(FilePermissions.Folder.Default, L("Permission:FolderManagement"));
            //folderPerm.AddChild(FilePermissions.Folder.Create, L("Permission:Create"));
            //folderPerm.AddChild(FilePermissions.Folder.Delete, L("Permission:Delete"));
            //folderPerm.AddChild(FilePermissions.Folder.Update, L("Permission:Update"));
            //folderPerm.AddChild(FilePermissions.Folder.ManagePermissions, L("Permission:ChangePermissions"));

            var ossConfigPerm = moduleGroup.AddPermission(FilePermissions.OssConfig.Default, L("Permission:OssConfig"));
            ossConfigPerm.AddChild(FilePermissions.OssConfig.Create, L("Permission:Create"));
            ossConfigPerm.AddChild(FilePermissions.OssConfig.Delete, L("Permission:Delete"));
            ossConfigPerm.AddChild(FilePermissions.OssConfig.Enable, L("Permission:Enable"));
            ossConfigPerm.AddChild(FilePermissions.OssConfig.Update, L("Permission:Update"));
            //ossConfigPerm.AddChild(FilePermissions.OssConfig.ManagePermissions, L("Permission:ChangePermissions"));

            //var tagPerm = moduleGroup.AddPermission(FilePermissions.Tag.Default, L("Permission:TagManagement"));
            //tagPerm.AddChild(FilePermissions.Tag.Create, L("Permission:Create"));
            //tagPerm.AddChild(FilePermissions.Tag.Delete, L("Permission:Delete"));
            //tagPerm.AddChild(FilePermissions.Tag.Update, L("Permission:Update"));
            //tagPerm.AddChild(FilePermissions.Tag.ManagePermissions, L("Permission:ChangePermissions"));


        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FileResource>(name);
        }
    }
}