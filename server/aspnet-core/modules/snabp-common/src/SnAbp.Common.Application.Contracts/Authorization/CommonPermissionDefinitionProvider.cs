using SnAbp.Common.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Common.Authorization
{
    public class CommonPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            //var moduleGroup = context.AddGroup(CommonPermissions.GroupName,L(CommonPermissions.GroupName));
            //var orgsPerm = moduleGroup.AddPermission(CommonPermissions.Orgs_Organization, L(CommonPermissions.Orgs_Organization));
            //orgsPerm.AddChild(CommonPermissions.Orgs_Organization_Create, L(CommonPermissions.Orgs_Organization_Create));
            //orgsPerm.AddChild(CommonPermissions.Orgs_Organization_Delete, L(CommonPermissions.Orgs_Organization_Delete));
            //orgsPerm.AddChild(CommonPermissions.Orgs_Organization_Update, L(CommonPermissions.Orgs_Organization_Update));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CommonResource>(name);
        }
    }
}