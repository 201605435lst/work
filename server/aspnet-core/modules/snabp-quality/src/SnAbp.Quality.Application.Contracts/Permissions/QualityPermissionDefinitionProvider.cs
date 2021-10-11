using SnAbp.Quality.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Quality.Permissions
{
    public class QualityPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(QualityPermissions.GroupName, L(QualityPermissions.GroupName));
            var qualityProblemPerm = moduleGroup.AddPermission(QualityPermissions.QualityProblems.Default, L("Permission:QualityProblems"));
            qualityProblemPerm.AddChild(QualityPermissions.QualityProblems.Create, L("Permission:Create"));
            qualityProblemPerm.AddChild(QualityPermissions.QualityProblems.Improve, L("Permission:Improve"));
            qualityProblemPerm.AddChild(QualityPermissions.QualityProblems.Verify, L("Permission:Verify"));
            qualityProblemPerm.AddChild(QualityPermissions.QualityProblems.Detail, L("Permission:Detail"));
            qualityProblemPerm.AddChild(QualityPermissions.QualityProblems.Delete, L("Permission:Delete"));
            qualityProblemPerm.AddChild(QualityPermissions.QualityProblems.Update, L("Permission:Update"));
            qualityProblemPerm.AddChild(QualityPermissions.QualityProblems.Export, L("Permission:Export"));
            //qualityProblemPerm.AddChild(QualityPermissions.QualityProblems.Position, L("Permission:Position"));

            var qualityProblemLibraryPerm = moduleGroup.AddPermission(QualityPermissions.QualityProblemLibraries.Default, L("Permission:QualityProblemLibraries"));
            qualityProblemLibraryPerm.AddChild(QualityPermissions.QualityProblemLibraries.Create, L("Permission:Create"));
            qualityProblemLibraryPerm.AddChild(QualityPermissions.QualityProblemLibraries.Update, L("Permission:Update"));
            qualityProblemLibraryPerm.AddChild(QualityPermissions.QualityProblemLibraries.Detail, L("Permission:Detail"));
            qualityProblemLibraryPerm.AddChild(QualityPermissions.QualityProblemLibraries.Delete, L("Permission:Delete"));
            qualityProblemLibraryPerm.AddChild(QualityPermissions.QualityProblemLibraries.Export, L("Permission:Export"));
            qualityProblemLibraryPerm.AddChild(QualityPermissions.QualityProblemLibraries.Import, L("Permission:Import"));
           

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<QualityResource>(name);
        }
    }
}