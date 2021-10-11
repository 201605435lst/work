using SnAbp.Technology.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Technology.Permissions
{
    public class TechnologyPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {

            var projectGroup = context.AddGroup(TechnologyPermissions.GroupName, L(TechnologyPermissions.GroupName));

            var projectPerm = projectGroup.AddPermission(TechnologyPermissions.ConstructInterface.Default, L("Permission:ConstructInterfaces"));
            projectPerm.AddChild(TechnologyPermissions.ConstructInterface.Import, L("Permission:Import"));
            projectPerm.AddChild(TechnologyPermissions.ConstructInterface.Reform, L("Permission:Reform"));
            projectPerm.AddChild(TechnologyPermissions.ConstructInterface.Sign, L("Permission:Sign"));
            projectPerm.AddChild(TechnologyPermissions.ConstructInterface.Detail, L("Permission:Detail"));
            projectPerm.AddChild(TechnologyPermissions.ConstructInterface.Export, L("Permission:Export"));

            var materialPlanPerm = projectGroup.AddPermission(TechnologyPermissions.MaterialPlan.Default, L("Permission:MaterialPlans"));
            materialPlanPerm.AddChild(TechnologyPermissions.MaterialPlan.Create, L("Permission:Create"));
            materialPlanPerm.AddChild(TechnologyPermissions.MaterialPlan.Update, L("Permission:Update"));
            materialPlanPerm.AddChild(TechnologyPermissions.MaterialPlan.Detail, L("Permission:Detail"));
            materialPlanPerm.AddChild(TechnologyPermissions.MaterialPlan.Export, L("Permission:Export"));
            materialPlanPerm.AddChild(TechnologyPermissions.MaterialPlan.Approval, L("Permission:Approval"));
            materialPlanPerm.AddChild(TechnologyPermissions.MaterialPlan.Delete, L("Permission:Delete"));
            materialPlanPerm.AddChild(TechnologyPermissions.MaterialPlan.Submit, L("Permission:Submit"));

            var quanityPerm = projectGroup.AddPermission(TechnologyPermissions.Quanity.Default, L("Permission:Quanitys"));
            quanityPerm.AddChild(TechnologyPermissions.Quanity.Statistic, L("Permission:Statistic"));
            quanityPerm.AddChild(TechnologyPermissions.Quanity.GenerateMaterialPlan, L("Permission:GenerateMaterialPlan"));
            quanityPerm.AddChild(TechnologyPermissions.Quanity.Export, L("Permission:Export"));

            var componentRltQRCodePerm = projectGroup.AddPermission(TechnologyPermissions.ComponentRltQRCode.Default, L("Permission:ComponentRltQRCodes"));
            componentRltQRCodePerm.AddChild(TechnologyPermissions.ComponentRltQRCode.GenerateCode, L("Permission:GenerateCode"));
            componentRltQRCodePerm.AddChild(TechnologyPermissions.ComponentRltQRCode.ExportCode, L("Permission:ExportCode"));
            componentRltQRCodePerm.AddChild(TechnologyPermissions.ComponentRltQRCode.CodeDetail, L("Permission:CodeDetail"));

            var disclosePerm = projectGroup.AddPermission(TechnologyPermissions.Disclose.Default, L("Permission:Discloses"));
            disclosePerm.AddChild(TechnologyPermissions.Disclose.Upload, L("Permission:Upload"));
            disclosePerm.AddChild(TechnologyPermissions.Disclose.Update, L("Permission:Update"));
            disclosePerm.AddChild(TechnologyPermissions.Disclose.Detail, L("Permission:Detail"));
            disclosePerm.AddChild(TechnologyPermissions.Disclose.Export, L("Permission:Export"));
            disclosePerm.AddChild(TechnologyPermissions.Disclose.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<TechnologyResource>(name);
        }
    }
}