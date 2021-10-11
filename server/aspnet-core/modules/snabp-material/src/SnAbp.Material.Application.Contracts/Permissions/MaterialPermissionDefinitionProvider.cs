using SnAbp.Material.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Material.Permissions
{
    public class MaterialPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(MaterialPermissions.GroupName, L(MaterialPermissions.GroupName));

            var supplierPerm = moduleGroup.AddPermission(MaterialPermissions.Supplier.Default, L("Permission:Suppliers"));
            supplierPerm.AddChild(MaterialPermissions.Supplier.Create, L("Permission:Create"));
            supplierPerm.AddChild(MaterialPermissions.Supplier.Update, L("Permission:Update"));
            supplierPerm.AddChild(MaterialPermissions.Supplier.Detail, L("Permission:Detail"));
            supplierPerm.AddChild(MaterialPermissions.Supplier.Delete, L("Permission:Delete"));
            supplierPerm.AddChild(MaterialPermissions.Supplier.Export, L("Permission:Export"));

            var inventoryPerm = moduleGroup.AddPermission(MaterialPermissions.Inventory.Default, L("Permission:Inventories"));
            inventoryPerm.AddChild(MaterialPermissions.Inventory.Detail, L("Permission:Detail"));
            inventoryPerm.AddChild(MaterialPermissions.Inventory.Export, L("Permission:Export"));

            var entryRecordPerm = moduleGroup.AddPermission(MaterialPermissions.EntryRecord.Default, L("Permission:EntryRecords"));
            entryRecordPerm.AddChild(MaterialPermissions.EntryRecord.Create, L("Permission:Create"));
            entryRecordPerm.AddChild(MaterialPermissions.EntryRecord.Detail, L("Permission:Detail"));
            entryRecordPerm.AddChild(MaterialPermissions.EntryRecord.Export, L("Permission:Export"));

            var outRecordPerm = moduleGroup.AddPermission(MaterialPermissions.OutRecord.Default, L("Permission:OutRecords"));
            outRecordPerm.AddChild(MaterialPermissions.OutRecord.Create, L("Permission:Create"));
            outRecordPerm.AddChild(MaterialPermissions.OutRecord.Detail, L("Permission:Detail"));
            outRecordPerm.AddChild(MaterialPermissions.OutRecord.Export, L("Permission:Export"));

            var constructionTeamPerm = moduleGroup.AddPermission(MaterialPermissions.ConstructionTeam.Default, L("Permission:ConstructionTeams"));
            constructionTeamPerm.AddChild(MaterialPermissions.ConstructionTeam.Create, L("Permission:Create"));
            constructionTeamPerm.AddChild(MaterialPermissions.ConstructionTeam.Update, L("Permission:Update"));
            constructionTeamPerm.AddChild(MaterialPermissions.ConstructionTeam.Detail, L("Permission:Detail"));
            constructionTeamPerm.AddChild(MaterialPermissions.ConstructionTeam.Delete, L("Permission:Delete"));
            constructionTeamPerm.AddChild(MaterialPermissions.ConstructionTeam.Export, L("Permission:Export"));
            constructionTeamPerm.AddChild(MaterialPermissions.ConstructionTeam.Import, L("Permission:Import"));


            var constructionSectionPerm = moduleGroup.AddPermission(MaterialPermissions.ConstructionSection.Default, L("Permission:ConstructionSections"));
            constructionSectionPerm.AddChild(MaterialPermissions.ConstructionSection.Create, L("Permission:Create"));
            constructionSectionPerm.AddChild(MaterialPermissions.ConstructionSection.Update, L("Permission:Update"));
            constructionSectionPerm.AddChild(MaterialPermissions.ConstructionSection.Detail, L("Permission:Detail"));
            constructionSectionPerm.AddChild(MaterialPermissions.ConstructionSection.Delete, L("Permission:Delete"));


            var MaterialnPerm = moduleGroup.AddPermission(MaterialPermissions.Material.Default, L("Permission:Materials"));
            MaterialnPerm.AddChild(MaterialPermissions.Material.Create, L("Permission:Create"));
            MaterialnPerm.AddChild(MaterialPermissions.Material.Update, L("Permission:Update"));
            MaterialnPerm.AddChild(MaterialPermissions.Material.Detail, L("Permission:Detail"));
            MaterialnPerm.AddChild(MaterialPermissions.Material.Delete, L("Permission:Delete"));
            MaterialnPerm.AddChild(MaterialPermissions.Material.Export, L("Permission:Export"));
            MaterialnPerm.AddChild(MaterialPermissions.Material.ExportCode, L("Permission:ExportCode"));
            MaterialnPerm.AddChild(MaterialPermissions.Material.Synchronize, L("Permission:Synchronize"));

            var inquirePerm = moduleGroup.AddPermission(MaterialPermissions.Inquire.Default, L("Permission:Inquires"));
            inquirePerm.AddChild(MaterialPermissions.Inquire.Detail, L("Permission:Detail"));
            inquirePerm.AddChild(MaterialPermissions.Inquire.Export, L("Permission:Export"));

            var MaterialPlansPerm = moduleGroup.AddPermission(MaterialPermissions.MaterialPlan.Default, L("Permission:MaterialPlans"));
            MaterialPlansPerm.AddChild(MaterialPermissions.MaterialPlan.GenerateMaterialPlan, L("Permission:GenerateMaterialPlan"));

            //var usePlanPerm = moduleGroup.AddPermission(MaterialPermissions.UsePlan.Default, L("Permission:UsePlans"));
            //usePlanPerm.AddChild(MaterialPermissions.UsePlan.Create, L("Permission:Create"));
            //usePlanPerm.AddChild(MaterialPermissions.UsePlan.Update, L("Permission:Update"));
            //usePlanPerm.AddChild(MaterialPermissions.UsePlan.Detail, L("Permission:Detail"));
            //usePlanPerm.AddChild(MaterialPermissions.UsePlan.Delete, L("Permission:Delete"));
            //usePlanPerm.AddChild(MaterialPermissions.UsePlan.Export, L("Permission:Export"));
            //usePlanPerm.AddChild(MaterialPermissions.UsePlan.Flow, L("Permission:Flow"));
            //usePlanPerm.AddChild(MaterialPermissions.UsePlan.Approval, L("Permission:Approval"));
            var purchasePlanPerm = moduleGroup.AddPermission(MaterialPermissions.PurchasePlan.Default, L("Permission:PurchasePlans"));
            purchasePlanPerm.AddChild(MaterialPermissions.PurchasePlan.Create, L("Permission:Create"));
            purchasePlanPerm.AddChild(MaterialPermissions.PurchasePlan.Update, L("Permission:Update"));
            purchasePlanPerm.AddChild(MaterialPermissions.PurchasePlan.Detail, L("Permission:Detail"));
            purchasePlanPerm.AddChild(MaterialPermissions.PurchasePlan.Delete, L("Permission:Delete"));
            purchasePlanPerm.AddChild(MaterialPermissions.PurchasePlan.Export, L("Permission:Export"));
            purchasePlanPerm.AddChild(MaterialPermissions.PurchasePlan.Approval, L("Permission:Approval"));
            var purchaseListPerm = moduleGroup.AddPermission(MaterialPermissions.PurchaseList.Default, L("Permission:PurchaseLists"));
            purchaseListPerm.AddChild(MaterialPermissions.PurchaseList.Create, L("Permission:Create"));
            purchaseListPerm.AddChild(MaterialPermissions.PurchaseList.Update, L("Permission:Update"));
            purchaseListPerm.AddChild(MaterialPermissions.PurchaseList.Detail, L("Permission:Detail"));
            purchaseListPerm.AddChild(MaterialPermissions.PurchaseList.Delete, L("Permission:Delete"));
            purchaseListPerm.AddChild(MaterialPermissions.PurchaseList.Export, L("Permission:Export"));
            purchaseListPerm.AddChild(MaterialPermissions.PurchaseList.Approval, L("Permission:Approval"));

            var materialAcceptancePerm = moduleGroup.AddPermission(MaterialPermissions.MaterialAcceptance.Default, L("Permission:MaterialAcceptances"));
            materialAcceptancePerm.AddChild(MaterialPermissions.MaterialAcceptance.Create, L("Permission:Create"));
            materialAcceptancePerm.AddChild(MaterialPermissions.MaterialAcceptance.Update, L("Permission:Update"));
            materialAcceptancePerm.AddChild(MaterialPermissions.MaterialAcceptance.Detail, L("Permission:Detail"));
            materialAcceptancePerm.AddChild(MaterialPermissions.MaterialAcceptance.Delete, L("Permission:Delete"));
            materialAcceptancePerm.AddChild(MaterialPermissions.MaterialAcceptance.Export, L("Permission:Export"));

            var materialOfBillPerm = moduleGroup.AddPermission(MaterialPermissions.MaterialOfBill.Default, L("Permission:MaterialOfBill"));
            materialOfBillPerm.AddChild(MaterialPermissions.MaterialOfBill.Create, L("Permission:Create"));
            materialOfBillPerm.AddChild(MaterialPermissions.MaterialOfBill.Update, L("Permission:Update"));
            materialOfBillPerm.AddChild(MaterialPermissions.MaterialOfBill.Detail, L("Permission:Detail"));
            materialOfBillPerm.AddChild(MaterialPermissions.MaterialOfBill.Delete, L("Permission:Delete"));
            materialOfBillPerm.AddChild(MaterialPermissions.MaterialOfBill.Export, L("Permission:Export"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MaterialResource>(name);
        }
    }
}