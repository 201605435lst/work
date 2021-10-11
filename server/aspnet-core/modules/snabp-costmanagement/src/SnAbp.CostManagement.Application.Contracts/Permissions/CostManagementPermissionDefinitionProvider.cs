using SnAbp.CostManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.CostManagement.Permissions
{
    public class CostManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(CostManagementPermissions.GroupName, L(CostManagementPermissions.GroupName));

            var costOtherPerm = moduleGroup.AddPermission(CostManagementPermissions.CostOther.Default, L("Permission:CostOthers"));
            costOtherPerm.AddChild(CostManagementPermissions.CostOther.Create, L("Permission:Create"));
            costOtherPerm.AddChild(CostManagementPermissions.CostOther.Delete, L("Permission:Delete"));
            costOtherPerm.AddChild(CostManagementPermissions.CostOther.Update, L("Permission:Update"));

            var peopleCostPerm = moduleGroup.AddPermission(CostManagementPermissions.PeopleCost.Default, L("Permission:PeopleCosts"));
            peopleCostPerm.AddChild(CostManagementPermissions.PeopleCost.Create, L("Permission:Create"));
            peopleCostPerm.AddChild(CostManagementPermissions.PeopleCost.Delete, L("Permission:Delete"));
            peopleCostPerm.AddChild(CostManagementPermissions.PeopleCost.Update, L("Permission:Update"));

            var moneyListPerm = moduleGroup.AddPermission(CostManagementPermissions.MoneyList.Default, L("Permission:MoneyLists"));
            moneyListPerm.AddChild(CostManagementPermissions.MoneyList.Create, L("Permission:Create"));
            moneyListPerm.AddChild(CostManagementPermissions.MoneyList.Delete, L("Permission:Delete"));
            moneyListPerm.AddChild(CostManagementPermissions.MoneyList.Update, L("Permission:Update"));

            var ContractPerm = moduleGroup.AddPermission(CostManagementPermissions.Contract.Default, L("Permission:Contracts"));
            ContractPerm.AddChild(CostManagementPermissions.Contract.Create, L("Permission:Create"));
            ContractPerm.AddChild(CostManagementPermissions.Contract.Delete, L("Permission:Delete"));
            ContractPerm.AddChild(CostManagementPermissions.Contract.Update, L("Permission:Update"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CostManagementResource>(name);
        }
    }
}