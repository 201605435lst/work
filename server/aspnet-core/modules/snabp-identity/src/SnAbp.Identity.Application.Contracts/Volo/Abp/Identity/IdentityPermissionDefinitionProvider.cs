using Volo.Abp.Authorization.Permissions;
using SnAbp.Identity.Localization;
using Volo.Abp.Localization;

namespace SnAbp.Identity
{
    public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityGroup = context.AddGroup(IdentityPermissions.GroupName, L(IdentityPermissions.GroupName));  

            var rolesPermission = identityGroup.AddPermission(IdentityPermissions.Roles.Default, L("Permission:RoleAuthorization"));
            rolesPermission.AddChild(IdentityPermissions.Roles.Create, L("Permission:Create"));
            rolesPermission.AddChild(IdentityPermissions.Roles.Update, L("Permission:Update"));
            rolesPermission.AddChild(IdentityPermissions.Roles.Delete, L("Permission:Delete"));
            //rolesPermission.AddChild(IdentityPermissions.Roles.Authorization, L("Permission:Authorization"));
            rolesPermission.AddChild(IdentityPermissions.Roles.ManagePermissions, L("Permission:ChangePermissions"));

            var usersPermission = identityGroup.AddPermission(IdentityPermissions.Users.Default, L("Permission:UserManagement"));
            usersPermission.AddChild(IdentityPermissions.Users.Create, L("Permission:Create"));
            usersPermission.AddChild(IdentityPermissions.Users.Update, L("Permission:Update"));
            usersPermission.AddChild(IdentityPermissions.Users.Delete, L("Permission:Delete"));
            usersPermission.AddChild(IdentityPermissions.Users.Import, L("Permission:Import"));
            usersPermission.AddChild(IdentityPermissions.Users.Export, L("Permission:Export"));
            usersPermission.AddChild(IdentityPermissions.Users.AssignRoles, L("Permission:AssignRoles"));
            usersPermission.AddChild(IdentityPermissions.Users.Detail, L("Permission:Detail"));
            usersPermission.AddChild(IdentityPermissions.Users.Reset, L("Permission:Reset"));
            //usersPermission.AddChild(IdentityPermissions.Users.ManagePermissions, L("Permission:ChangePermissions"));

            identityGroup
                .AddPermission(IdentityPermissions.UserLookup.Default, L("Permission:UserLookup"))
                .WithProviders(ClientPermissionValueProvider.ProviderName);

            // 添加组织机构的权限定义
            var organizationPermission = identityGroup.AddPermission(IdentityPermissions.Organization.Default,
                L("Permission:OrganizationUnit"));
            organizationPermission.AddChild(IdentityPermissions.Organization.Create, L("Permission:Create"));
            organizationPermission.AddChild(IdentityPermissions.Organization.Update, L("Permission:Update"));
            organizationPermission.AddChild(IdentityPermissions.Organization.Delete, L("Permission:Delete"));
            organizationPermission.AddChild(IdentityPermissions.Organization.Import, L("Permission:Import"));
            organizationPermission.AddChild(IdentityPermissions.Organization.Export, L("Permission:Export"));
            //organizationPermission.AddChild(IdentityPermissions.Organization.ManagePermissions, L("Permission:ChangePermissions"));

            // 数据字典
            var dataDictionaryPermission = identityGroup.AddPermission(IdentityPermissions.DataDictionary.Default,
                L("Permission:DataDictionary"));
            dataDictionaryPermission.AddChild(IdentityPermissions.DataDictionary.Create, L("Permission:Create"));
            dataDictionaryPermission.AddChild(IdentityPermissions.DataDictionary.Update, L("Permission:Update"));
            dataDictionaryPermission.AddChild(IdentityPermissions.DataDictionary.Delete, L("Permission:Delete"));
            //dataDictionaryPermission.AddChild(IdentityPermissions.DataDictionary.ManagePermissions, L("Permission:ChangePermissions"));
        }

        private static LocalizableString L(string name) => LocalizableString.Create<IdentityResource>(name);
    }
}