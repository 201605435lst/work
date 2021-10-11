using SnAbp.Basic.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Basic.Authorization
{
    public class BasicPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(BasicPermissions.GroupName, L("Permission:AbpBasic"));

            var railwayPermission = moduleGroup.AddPermission(BasicPermissions.Railway.Default, L("Permission:Railways"));
            railwayPermission.AddChild(BasicPermissions.Railway.Create, L("Permission:Create"));
            railwayPermission.AddChild(BasicPermissions.Railway.Delete, L("Permission:Delete"));
            railwayPermission.AddChild(BasicPermissions.Railway.Update, L("Permission:Update"));
            railwayPermission.AddChild(BasicPermissions.Railway.Relate, L("Permission:Relate"));
            railwayPermission.AddChild(BasicPermissions.Railway.Detail, L("Permission:Detail"));
            railwayPermission.AddChild(BasicPermissions.Railway.Import, L("Permission:Import"));
            railwayPermission.AddChild(BasicPermissions.Railway.Export, L("Permission:Export"));

            var stationPermission = moduleGroup.AddPermission(BasicPermissions.Station.Default, L("Permission:Stations"));
            stationPermission.AddChild(BasicPermissions.Station.Create, L("Permission:Create"));
            stationPermission.AddChild(BasicPermissions.Station.Delete, L("Permission:Delete"));
            stationPermission.AddChild(BasicPermissions.Station.Update, L("Permission:Update"));
            stationPermission.AddChild(BasicPermissions.Station.Detail, L("Permission:Detail"));
            stationPermission.AddChild(BasicPermissions.Station.Import, L("Permission:Import"));
            stationPermission.AddChild(BasicPermissions.Station.Export, L("Permission:Export"));

            var installationSitePermission = moduleGroup.AddPermission(BasicPermissions.InstallationSite.Default, L("Permission:InstallationSites"));
            installationSitePermission.AddChild(BasicPermissions.InstallationSite.Create, L("Permission:Create"));
            installationSitePermission.AddChild(BasicPermissions.InstallationSite.Delete, L("Permission:Delete"));
            installationSitePermission.AddChild(BasicPermissions.InstallationSite.Update, L("Permission:Update"));
            installationSitePermission.AddChild(BasicPermissions.InstallationSite.Detail, L("Permission:Detail"));
            installationSitePermission.AddChild(BasicPermissions.InstallationSite.Import, L("Permission:Import"));
            installationSitePermission.AddChild(BasicPermissions.InstallationSite.Export, L("Permission:Export"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BasicResource>(name);
        }
    }
}