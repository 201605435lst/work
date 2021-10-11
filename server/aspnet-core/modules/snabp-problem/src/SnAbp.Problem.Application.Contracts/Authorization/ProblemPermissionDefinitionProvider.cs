using SnAbp.Problem.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Problem.Authorization
{
    public class ProblemPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(ProblemPermissions.GroupName, L(ProblemPermissions.GroupName));

            var problemPerm = moduleGroup.AddPermission(ProblemPermissions.Problem.Default, L("Permission:Problems"));
            problemPerm.AddChild(ProblemPermissions.Problem.Create, L("Permission:Create"));
            problemPerm.AddChild(ProblemPermissions.Problem.Update, L("Permission:Update"));
            problemPerm.AddChild(ProblemPermissions.Problem.Detail, L("Permission:Detail"));
            problemPerm.AddChild(ProblemPermissions.Problem.Delete, L("Permission:Delete"));

            var problemCategoryPerm = moduleGroup.AddPermission(ProblemPermissions.ProblemCategory.Default, L("Permission:ProblemCategories"));
            problemCategoryPerm.AddChild(ProblemPermissions.ProblemCategory.Create, L("Permission:Create"));
            problemCategoryPerm.AddChild(ProblemPermissions.ProblemCategory.Update, L("Permission:Update"));
            problemCategoryPerm.AddChild(ProblemPermissions.ProblemCategory.Detail, L("Permission:Detail"));
            problemCategoryPerm.AddChild(ProblemPermissions.ProblemCategory.Delete, L("Permission:Delete"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ProblemResource>(name);
        }
    }
}