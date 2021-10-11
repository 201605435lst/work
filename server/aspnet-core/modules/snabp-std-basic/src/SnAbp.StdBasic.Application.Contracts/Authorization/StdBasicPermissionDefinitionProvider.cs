using SnAbp.StdBasic.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.StdBasic.Authorization
{
    public class StdBasicPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {

            var moduleGroup = context.AddGroup(StdBasicPermissions.GroupName, L(StdBasicPermissions.GroupName));

            var componentCategoryPerm = moduleGroup.AddPermission(StdBasicPermissions.ComponentCategory.Default, L("Permission:ComponentCategories"));
            componentCategoryPerm.AddChild(StdBasicPermissions.ComponentCategory.Create, L("Permission:Create"));
            componentCategoryPerm.AddChild(StdBasicPermissions.ComponentCategory.Delete, L("Permission:Delete"));
            componentCategoryPerm.AddChild(StdBasicPermissions.ComponentCategory.Update, L("Permission:Update"));
            componentCategoryPerm.AddChild(StdBasicPermissions.ComponentCategory.Detail, L("Permission:Detail"));
            componentCategoryPerm.AddChild(StdBasicPermissions.ComponentCategory.Import, L("Permission:Import"));
            componentCategoryPerm.AddChild(StdBasicPermissions.ComponentCategory.Export, L("Permission:Export"));

            var productCategoryPerm = moduleGroup.AddPermission(StdBasicPermissions.ProductCategory.Default, L("Permission:ProductCategories"));
            productCategoryPerm.AddChild(StdBasicPermissions.ProductCategory.Create, L("Permission:Create"));
            productCategoryPerm.AddChild(StdBasicPermissions.ProductCategory.Delete, L("Permission:Delete"));
            productCategoryPerm.AddChild(StdBasicPermissions.ProductCategory.Update, L("Permission:Update"));
            productCategoryPerm.AddChild(StdBasicPermissions.ProductCategory.Detail, L("Permission:Detail"));
            productCategoryPerm.AddChild(StdBasicPermissions.ProductCategory.Import, L("Permission:Import"));
            productCategoryPerm.AddChild(StdBasicPermissions.ProductCategory.Export, L("Permission:Export"));

            var manufacturePerm = moduleGroup.AddPermission(StdBasicPermissions.Manufacture.Default, L("Permission:Manufactures"));
            manufacturePerm.AddChild(StdBasicPermissions.Manufacture.Create, L("Permission:Create"));
            manufacturePerm.AddChild(StdBasicPermissions.Manufacture.Delete, L("Permission:Delete"));
            manufacturePerm.AddChild(StdBasicPermissions.Manufacture.Update, L("Permission:Update"));
            manufacturePerm.AddChild(StdBasicPermissions.Manufacture.Detail, L("Permission:Detail"));
            manufacturePerm.AddChild(StdBasicPermissions.Manufacture.Import, L("Permission:Import"));
            manufacturePerm.AddChild(StdBasicPermissions.Manufacture.Export, L("Permission:Export"));

            var standardEquipmentPerm = moduleGroup.AddPermission(StdBasicPermissions.StandardEquipment.Default, L("Permission:StandardEquipments"));
            standardEquipmentPerm.AddChild(StdBasicPermissions.StandardEquipment.Create, L("Permission:Create"));
            standardEquipmentPerm.AddChild(StdBasicPermissions.StandardEquipment.Delete, L("Permission:Delete"));
            standardEquipmentPerm.AddChild(StdBasicPermissions.StandardEquipment.Update, L("Permission:Update"));
            standardEquipmentPerm.AddChild(StdBasicPermissions.StandardEquipment.Detail, L("Permission:Detail"));
            standardEquipmentPerm.AddChild(StdBasicPermissions.StandardEquipment.Import, L("Permission:Import"));
            standardEquipmentPerm.AddChild(StdBasicPermissions.StandardEquipment.Export, L("Permission:Export"));

            var terminalPerm = moduleGroup.AddPermission(StdBasicPermissions.Terminal.Default, L("Permission:Terminals"));
            terminalPerm.AddChild(StdBasicPermissions.Terminal.Create, L("Permission:Create"));
            terminalPerm.AddChild(StdBasicPermissions.Terminal.Delete, L("Permission:Delete"));
            terminalPerm.AddChild(StdBasicPermissions.Terminal.Update, L("Permission:Update"));
            terminalPerm.AddChild(StdBasicPermissions.Terminal.Detail, L("Permission:Detail"));

            var repairGroupPerm = moduleGroup.AddPermission(StdBasicPermissions.RepairGroup.Default, L("Permission:RepairGroup"));
            repairGroupPerm.AddChild(StdBasicPermissions.RepairGroup.Create, L("Permission:Create"));
            repairGroupPerm.AddChild(StdBasicPermissions.RepairGroup.Delete, L("Permission:Delete"));
            repairGroupPerm.AddChild(StdBasicPermissions.RepairGroup.Update, L("Permission:Update"));
            repairGroupPerm.AddChild(StdBasicPermissions.RepairGroup.Detail, L("Permission:Detail"));

            var repairItemPerm = moduleGroup.AddPermission(StdBasicPermissions.RepairItem.Default, L("Permission:RepairItems"));
            repairItemPerm.AddChild(StdBasicPermissions.RepairItem.Create, L("Permission:Create"));
            repairItemPerm.AddChild(StdBasicPermissions.RepairItem.Delete, L("Permission:Delete"));
            repairItemPerm.AddChild(StdBasicPermissions.RepairItem.Update, L("Permission:Update"));
            repairItemPerm.AddChild(StdBasicPermissions.RepairItem.Detail, L("Permission:Detail"));
            repairItemPerm.AddChild(StdBasicPermissions.RepairItem.CreateTagMigration, L("Permission:CreateTagMigration"));

            var repairTestItemPerm = moduleGroup.AddPermission(StdBasicPermissions.RepairTestItem.Default, L("Permission:RepairTestItems"));
            repairTestItemPerm.AddChild(StdBasicPermissions.RepairTestItem.Create, L("Permission:Create"));
            repairTestItemPerm.AddChild(StdBasicPermissions.RepairTestItem.Delete, L("Permission:Delete"));
            repairTestItemPerm.AddChild(StdBasicPermissions.RepairTestItem.Update, L("Permission:Update"));
            repairTestItemPerm.AddChild(StdBasicPermissions.RepairTestItem.Detail, L("Permission:Detail"));
            repairTestItemPerm.AddChild(StdBasicPermissions.RepairTestItem.UpdateOrder, L("Permission:UpdateOrder"));
            repairTestItemPerm.AddChild(StdBasicPermissions.RepairTestItem.Upgrade, L("Permission:Upgrade"));

            var WorkAttentionPerm = moduleGroup.AddPermission(StdBasicPermissions.WorkAttention.Default, L("Permission:WorkAttention"));
            WorkAttentionPerm.AddChild(StdBasicPermissions.WorkAttention.Create, L("Permission:Create"));
            WorkAttentionPerm.AddChild(StdBasicPermissions.WorkAttention.Delete, L("Permission:Delete"));
            WorkAttentionPerm.AddChild(StdBasicPermissions.WorkAttention.Update, L("Permission:Update"));
            WorkAttentionPerm.AddChild(StdBasicPermissions.WorkAttention.Detail, L("Permission:Detail"));
            WorkAttentionPerm.AddChild(StdBasicPermissions.WorkAttention.CreateType, L("Permission:CreateType"));

            var influenceRangePerm = moduleGroup.AddPermission(StdBasicPermissions.InfluenceRange.Default, L("Permission:InfluenceRanges"));
            influenceRangePerm.AddChild(StdBasicPermissions.InfluenceRange.Create, L("Permission:Create"));
            influenceRangePerm.AddChild(StdBasicPermissions.InfluenceRange.Delete, L("Permission:Delete"));
            influenceRangePerm.AddChild(StdBasicPermissions.InfluenceRange.Update, L("Permission:Update"));
            influenceRangePerm.AddChild(StdBasicPermissions.InfluenceRange.Detail, L("Permission:Detail"));

            var MVDPropertyPerm = moduleGroup.AddPermission(StdBasicPermissions.MVDProperty.Default, L("Permission:MVDProperty"));
            MVDPropertyPerm.AddChild(StdBasicPermissions.MVDProperty.Create, L("Permission:Create"));
            MVDPropertyPerm.AddChild(StdBasicPermissions.MVDProperty.Delete, L("Permission:Delete"));
            MVDPropertyPerm.AddChild(StdBasicPermissions.MVDProperty.Update, L("Permission:Update"));
            MVDPropertyPerm.AddChild(StdBasicPermissions.MVDProperty.Detail, L("Permission:Detail"));
            MVDPropertyPerm.AddChild(StdBasicPermissions.MVDProperty.Import, L("Permission:Import"));
            MVDPropertyPerm.AddChild(StdBasicPermissions.MVDProperty.Export, L("Permission:Export"));

            var MVDCategoryPerm = moduleGroup.AddPermission(StdBasicPermissions.MVDCategory.Default, L("Permission:MVDCategory"));
            MVDCategoryPerm.AddChild(StdBasicPermissions.MVDCategory.Create, L("Permission:Create"));
            MVDCategoryPerm.AddChild(StdBasicPermissions.MVDCategory.Delete, L("Permission:Delete"));
            MVDCategoryPerm.AddChild(StdBasicPermissions.MVDCategory.Update, L("Permission:Update"));
            MVDCategoryPerm.AddChild(StdBasicPermissions.MVDCategory.Detail, L("Permission:Detail"));
            MVDCategoryPerm.AddChild(StdBasicPermissions.MVDCategory.Import, L("Permission:Import"));
            MVDCategoryPerm.AddChild(StdBasicPermissions.MVDCategory.Export, L("Permission:Export"));
            var individualProjectPerm = moduleGroup.AddPermission(StdBasicPermissions.IndividualProject.Default, L("Permission:IndividualProjects"));
            individualProjectPerm.AddChild(StdBasicPermissions.IndividualProject.Create, L("Permission:Create"));
            individualProjectPerm.AddChild(StdBasicPermissions.IndividualProject.Delete, L("Permission:Delete"));
            individualProjectPerm.AddChild(StdBasicPermissions.IndividualProject.Update, L("Permission:Update"));
            individualProjectPerm.AddChild(StdBasicPermissions.IndividualProject.Detail, L("Permission:Detail"));
            individualProjectPerm.AddChild(StdBasicPermissions.IndividualProject.Import, L("Permission:Import"));
            individualProjectPerm.AddChild(StdBasicPermissions.IndividualProject.Export, L("Permission:Export"));



            var processTemplatePerm = moduleGroup.AddPermission(StdBasicPermissions.ProcessTemplate.Default, L("Permission:ProcessTemplates"));
            processTemplatePerm.AddChild(StdBasicPermissions.ProcessTemplate.Create, L("Permission:Create"));
            processTemplatePerm.AddChild(StdBasicPermissions.ProcessTemplate.Delete, L("Permission:Delete"));
            processTemplatePerm.AddChild(StdBasicPermissions.ProcessTemplate.Update, L("Permission:Update"));
            processTemplatePerm.AddChild(StdBasicPermissions.ProcessTemplate.Detail, L("Permission:Detail"));
            processTemplatePerm.AddChild(StdBasicPermissions.ProcessTemplate.Import, L("Permission:Import"));
            processTemplatePerm.AddChild(StdBasicPermissions.ProcessTemplate.Export, L("Permission:Export"));

            var projectItemPerm = moduleGroup.AddPermission(StdBasicPermissions.ProjectItem.Default, L("Permission:ProjectItems"));
            projectItemPerm.AddChild(StdBasicPermissions.ProjectItem.Create, L("Permission:Create"));
            projectItemPerm.AddChild(StdBasicPermissions.ProjectItem.Delete, L("Permission:Delete"));
            projectItemPerm.AddChild(StdBasicPermissions.ProjectItem.Update, L("Permission:Update"));
            projectItemPerm.AddChild(StdBasicPermissions.ProjectItem.Detail, L("Permission:Detail"));
            projectItemPerm.AddChild(StdBasicPermissions.ProjectItem.Import, L("Permission:Import"));
            projectItemPerm.AddChild(StdBasicPermissions.ProjectItem.Export, L("Permission:Export"));

            var quotaCategoryPerm = moduleGroup.AddPermission(StdBasicPermissions.QuotaCategory.Default, L("Permission:QuotaCategorys"));
            quotaCategoryPerm.AddChild(StdBasicPermissions.QuotaCategory.Create, L("Permission:Create"));
            quotaCategoryPerm.AddChild(StdBasicPermissions.QuotaCategory.Delete, L("Permission:Delete"));
            quotaCategoryPerm.AddChild(StdBasicPermissions.QuotaCategory.Update, L("Permission:Update"));
            quotaCategoryPerm.AddChild(StdBasicPermissions.QuotaCategory.Detail, L("Permission:Detail"));
            quotaCategoryPerm.AddChild(StdBasicPermissions.QuotaCategory.Import, L("Permission:Import"));
            quotaCategoryPerm.AddChild(StdBasicPermissions.QuotaCategory.Export, L("Permission:Export"));

            var quotaPerm = moduleGroup.AddPermission(StdBasicPermissions.Quota.Default, L("Permission:Quotas"));
            quotaPerm.AddChild(StdBasicPermissions.Quota.Create, L("Permission:Create"));
            quotaPerm.AddChild(StdBasicPermissions.Quota.Delete, L("Permission:Delete"));
            quotaPerm.AddChild(StdBasicPermissions.Quota.Update, L("Permission:Update"));
            quotaPerm.AddChild(StdBasicPermissions.Quota.Detail, L("Permission:Detail"));
            quotaPerm.AddChild(StdBasicPermissions.Quota.Import, L("Permission:Import"));
            quotaPerm.AddChild(StdBasicPermissions.Quota.Export, L("Permission:Export"));

            var quotaItemPerm = moduleGroup.AddPermission(StdBasicPermissions.QuotaItem.Default, L("Permission:QuotaItems"));
            quotaItemPerm.AddChild(StdBasicPermissions.QuotaItem.Create, L("Permission:Create"));
            quotaItemPerm.AddChild(StdBasicPermissions.QuotaItem.Delete, L("Permission:Delete"));
            quotaItemPerm.AddChild(StdBasicPermissions.QuotaItem.Update, L("Permission:Update"));
            quotaItemPerm.AddChild(StdBasicPermissions.QuotaItem.Detail, L("Permission:Detail"));
            quotaItemPerm.AddChild(StdBasicPermissions.QuotaItem.Import, L("Permission:Import"));
            quotaItemPerm.AddChild(StdBasicPermissions.QuotaItem.Export, L("Permission:Export"));

            var basePricePerm = moduleGroup.AddPermission(StdBasicPermissions.BasePrice.Default, L("Permission:BasePrices"));
            basePricePerm.AddChild(StdBasicPermissions.BasePrice.Create, L("Permission:Create"));
            basePricePerm.AddChild(StdBasicPermissions.BasePrice.Delete, L("Permission:Delete"));
            basePricePerm.AddChild(StdBasicPermissions.BasePrice.Update, L("Permission:Update"));
            basePricePerm.AddChild(StdBasicPermissions.BasePrice.Detail, L("Permission:Detail"));
            basePricePerm.AddChild(StdBasicPermissions.BasePrice.Import, L("Permission:Import"));
            basePricePerm.AddChild(StdBasicPermissions.BasePrice.Export, L("Permission:Export"));

            var computerCodePerm = moduleGroup.AddPermission(StdBasicPermissions.ComputerCode.Default, L("Permission:ComputerCodes"));
            computerCodePerm.AddChild(StdBasicPermissions.ComputerCode.Create, L("Permission:Create"));
            computerCodePerm.AddChild(StdBasicPermissions.ComputerCode.Delete, L("Permission:Delete"));
            computerCodePerm.AddChild(StdBasicPermissions.ComputerCode.Update, L("Permission:Update"));
            computerCodePerm.AddChild(StdBasicPermissions.ComputerCode.Detail, L("Permission:Detail"));
            computerCodePerm.AddChild(StdBasicPermissions.ComputerCode.Import, L("Permission:Import"));
            computerCodePerm.AddChild(StdBasicPermissions.ComputerCode.Export, L("Permission:Export"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<StdBasicResource>(name);
        }
    }
}