using SnAbp.Project.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Project.Permissions
{
    public class ProjectPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var projectGroup = context.AddGroup(ProjectPermissions.GroupName, L(ProjectPermissions.GroupName));

            var projectPerm = projectGroup.AddPermission(ProjectPermissions.Project.Default, L("Permission:Projects"));
            projectPerm.AddChild(ProjectPermissions.Project.Create, L("Permission:Create"));
            projectPerm.AddChild(ProjectPermissions.Project.Delete, L("Permission:Delete"));
            projectPerm.AddChild(ProjectPermissions.Project.Update, L("Permission:Update"));
            projectPerm.AddChild(ProjectPermissions.Project.Detail, L("Permission:Detail"));
            projectPerm.AddChild(ProjectPermissions.Project.Export, L("Permission:Export"));
            var dossierPerm = projectGroup.AddPermission(ProjectPermissions.Dossier.Default, L("Permission:Dossier"));
            dossierPerm.AddChild(ProjectPermissions.Dossier.Create, L("Permission:Create"));
            dossierPerm.AddChild(ProjectPermissions.Dossier.Delete, L("Permission:Delete"));
            dossierPerm.AddChild(ProjectPermissions.Dossier.Update, L("Permission:Update"));
            dossierPerm.AddChild(ProjectPermissions.Dossier.Detail, L("Permission:Detail"));
            dossierPerm.AddChild(ProjectPermissions.Dossier.Export, L("Permission:Export"));
            dossierPerm.AddChild(ProjectPermissions.Dossier.Import, L("Permission:Import"));
            dossierPerm.AddChild(ProjectPermissions.Dossier.Download, L("Permission:Download"));

            var archivesPerm = projectGroup.AddPermission(ProjectPermissions.Archives.Default, L("Permission:Archives"));
            archivesPerm.AddChild(ProjectPermissions.Archives.Create, L("Permission:Create"));
            archivesPerm.AddChild(ProjectPermissions.Archives.Delete, L("Permission:Delete"));
            archivesPerm.AddChild(ProjectPermissions.Archives.Update, L("Permission:Update"));
            archivesPerm.AddChild(ProjectPermissions.Archives.Apply, L("Permission:Apply"));
            archivesPerm.AddChild(ProjectPermissions.Archives.Export, L("Permission:Export"));
            archivesPerm.AddChild(ProjectPermissions.Archives.Import, L("Permission:Import"));

            var archivesCategoryPerm = projectGroup.AddPermission(ProjectPermissions.ArchivesCategory.Default, L("Permission:ArchivesCategory"));
            archivesCategoryPerm.AddChild(ProjectPermissions.ArchivesCategory.Create, L("Permission:Create"));
            archivesCategoryPerm.AddChild(ProjectPermissions.ArchivesCategory.Delete, L("Permission:Delete"));
            archivesCategoryPerm.AddChild(ProjectPermissions.ArchivesCategory.Update, L("Permission:Update"));
            archivesCategoryPerm.AddChild(ProjectPermissions.ArchivesCategory.Encrypt, L("Permission:Encrypt"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ProjectResource>(name);
        }
    }
}