using SnAbp.Regulation.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Regulation.Permissions
{
    public class RegulationPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(RegulationPermissions.GroupName, L(RegulationPermissions.GroupName));

            var InstitutionPerm = myGroup.AddPermission(RegulationPermissions.Institution.Default, L("Permission:Institutions"));
            InstitutionPerm.AddChild(RegulationPermissions.Institution.Create, L("Permission:Create"));
            InstitutionPerm.AddChild(RegulationPermissions.Institution.Delete, L("Permission:Delete"));
            InstitutionPerm.AddChild(RegulationPermissions.Institution.Update, L("Permission:Update"));
            InstitutionPerm.AddChild(RegulationPermissions.Institution.Detail, L("Permission:Detail"));
            InstitutionPerm.AddChild(RegulationPermissions.Institution.Import, L("Permission:Import"));
            InstitutionPerm.AddChild(RegulationPermissions.Institution.Export, L("Permission:Export"));
            InstitutionPerm.AddChild(RegulationPermissions.Institution.Authority, L("Permission:Authority"));
            InstitutionPerm.AddChild(RegulationPermissions.Institution.Audit, L("Permission:Audit"));

            var LabelPerm = myGroup.AddPermission(RegulationPermissions.Label.Default, L("Permission:Labels"));
            LabelPerm.AddChild(RegulationPermissions.Label.Create, L("Permission:Create"));
            LabelPerm.AddChild(RegulationPermissions.Label.Delete, L("Permission:Delete"));
            LabelPerm.AddChild(RegulationPermissions.Label.Update, L("Permission:Update"));
            LabelPerm.AddChild(RegulationPermissions.Label.Detail, L("Permission:Detail"));
            LabelPerm.AddChild(RegulationPermissions.Label.Import, L("Permission:Import"));
            LabelPerm.AddChild(RegulationPermissions.Label.Export, L("Permission:Export"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<RegulationResource>(name);
        }
    }
}