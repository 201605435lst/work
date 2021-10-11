using SnAbp.Safe.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Safe.Permissions
{
    public class SafePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {

            var moduleGroup = context.AddGroup(SafePermissions.GroupName, L(SafePermissions.GroupName));

            var problemPerm = moduleGroup.AddPermission(SafePermissions.SafeProblem.Default, L("Permission:Problems"));
            problemPerm.AddChild(SafePermissions.SafeProblem.Detail, L("Permission:Detail"));
            problemPerm.AddChild(SafePermissions.SafeProblem.Export, L("Permission:Export"));
            problemPerm.AddChild(SafePermissions.SafeProblem.Sign, L("Permission:Sign"));
            problemPerm.AddChild(SafePermissions.SafeProblem.Reform, L("Permission:Reform"));
            problemPerm.AddChild(SafePermissions.SafeProblem.Verify, L("Permission:Verify"));
            problemPerm.AddChild(SafePermissions.SafeProblem.Update, L("Permission:Update"));
            //problemPerm.AddChild(SafePermissions.SafeProblem.Position, L("Permission:Position"));

            var problemLibraryPerm = moduleGroup.AddPermission(SafePermissions.SafeProblemLibrary.Default, L("Permission:ProblemLibrarys"));
      
            problemLibraryPerm.AddChild(SafePermissions.SafeProblemLibrary.Delete, L("Permission:Delete"));
            problemLibraryPerm.AddChild(SafePermissions.SafeProblemLibrary.Export, L("Permission:Export"));
            problemLibraryPerm.AddChild(SafePermissions.SafeProblemLibrary.Update, L("Permission:Update"));
            problemLibraryPerm.AddChild(SafePermissions.SafeProblemLibrary.Import, L("Permission:Import"));
            problemLibraryPerm.AddChild(SafePermissions.SafeProblemLibrary.Create, L("Permission:Create"));
        }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SafeResource>(name);
    }
}
}