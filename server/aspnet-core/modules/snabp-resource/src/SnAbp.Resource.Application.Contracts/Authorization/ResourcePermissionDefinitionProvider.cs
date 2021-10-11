using SnAbp.Resource.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Resource.Authorization
{
    public class ResourcePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {

            var moduleGroup = context.AddGroup(ResourcePermissions.GroupName, L(ResourcePermissions.GroupName));

            var cableExtendPerm = moduleGroup.AddPermission(ResourcePermissions.CableExtend.Default, L("Permission:CableExtens"));
            cableExtendPerm.AddChild(ResourcePermissions.CableExtend.Update, L("Permission:Update"));
            cableExtendPerm.AddChild(ResourcePermissions.CableExtend.Detail, L("Permission:Detail"));

            var cableLocationPerm = moduleGroup.AddPermission(ResourcePermissions.CableLocation.Default, L("Permission:CableLocations"));
            cableLocationPerm.AddChild(ResourcePermissions.CableLocation.Create, L("Permission:Create"));
            cableLocationPerm.AddChild(ResourcePermissions.CableLocation.Delete, L("Permission:Delete"));
            cableLocationPerm.AddChild(ResourcePermissions.CableLocation.Update, L("Permission:Update"));
            cableLocationPerm.AddChild(ResourcePermissions.CableLocation.Detail, L("Permission:Detail"));

            var equipmentPerm = moduleGroup.AddPermission(ResourcePermissions.Equipment.Default, L("Permission:Equipments"));
            equipmentPerm.AddChild(ResourcePermissions.Equipment.Create, L("Permission:Create"));
            equipmentPerm.AddChild(ResourcePermissions.Equipment.Delete, L("Permission:Delete"));
            equipmentPerm.AddChild(ResourcePermissions.Equipment.Update, L("Permission:Update"));
            equipmentPerm.AddChild(ResourcePermissions.Equipment.Detail, L("Permission:Detail"));
            equipmentPerm.AddChild(ResourcePermissions.Equipment.Import, L("Permission:Import"));
            equipmentPerm.AddChild(ResourcePermissions.Equipment.Export, L("Permission:Export"));
            equipmentPerm.AddChild(ResourcePermissions.Equipment.GenerateQuality, L("Permission:GenerateQuality"));     
            equipmentPerm.AddChild(ResourcePermissions.Equipment.WaitingStorage, L("Permission:WaitingStorage"));

            var equipmentGroupPerm = moduleGroup.AddPermission(ResourcePermissions.EquipmentGroup.Default, L("Permission:EquipmentGroups"));
            equipmentGroupPerm.AddChild(ResourcePermissions.EquipmentGroup.Create, L("Permission:Create"));
            equipmentGroupPerm.AddChild(ResourcePermissions.EquipmentGroup.Delete, L("Permission:Delete"));
            equipmentGroupPerm.AddChild(ResourcePermissions.EquipmentGroup.Update, L("Permission:Update"));
            equipmentGroupPerm.AddChild(ResourcePermissions.EquipmentGroup.Detail, L("Permission:Detail"));
            equipmentGroupPerm.AddChild(ResourcePermissions.EquipmentGroup.Import, L("Permission:Import"));
            equipmentGroupPerm.AddChild(ResourcePermissions.EquipmentGroup.Export, L("Permission:Export"));

            var equipmentPropertyPerm = moduleGroup.AddPermission(ResourcePermissions.EquipmentProperty.Default, L("Permission:EquipmentProperties"));
            equipmentPropertyPerm.AddChild(ResourcePermissions.EquipmentProperty.Create, L("Permission:Create"));
            equipmentPropertyPerm.AddChild(ResourcePermissions.EquipmentProperty.Delete, L("Permission:Delete"));
            equipmentPropertyPerm.AddChild(ResourcePermissions.EquipmentProperty.Update, L("Permission:Update"));

            var storeEquipmentPerm = moduleGroup.AddPermission(ResourcePermissions.StoreEquipment.Default, L("Permission:StoreEquipments"));
            storeEquipmentPerm.AddChild(ResourcePermissions.StoreEquipment.Create, L("Permission:Create"));
            storeEquipmentPerm.AddChild(ResourcePermissions.StoreEquipment.Export, L("Permission:Export"));

            var storeEquipmentTestPerm = moduleGroup.AddPermission(ResourcePermissions.StoreEquipmentTest.Default, L("Permission:StoreEquipmentTest"));
            storeEquipmentTestPerm.AddChild(ResourcePermissions.StoreEquipmentTest.Create, L("Permission:Create"));
            storeEquipmentTestPerm.AddChild(ResourcePermissions.StoreEquipmentTest.Detail, L("Permission:Detail"));

            var storeEquipmentTransferPerm = moduleGroup.AddPermission(ResourcePermissions.StoreEquipmentTransfer.Default, L("Permission:StoreEquipmentTransfer"));
            storeEquipmentTransferPerm.AddChild(ResourcePermissions.StoreEquipmentTransfer.Create, L("Permission:Create"));
            storeEquipmentTransferPerm.AddChild(ResourcePermissions.StoreEquipmentTransfer.Detail, L("Permission:Detail"));

            var storeHousePerm = moduleGroup.AddPermission(ResourcePermissions.StoreHouse.Default, L("Permission:StoreHouse"));
            storeHousePerm.AddChild(ResourcePermissions.StoreHouse.Create, L("Permission:Create"));
            storeHousePerm.AddChild(ResourcePermissions.StoreHouse.Delete, L("Permission:Delete"));
            storeHousePerm.AddChild(ResourcePermissions.StoreHouse.Update, L("Permission:Update"));
            storeHousePerm.AddChild(ResourcePermissions.StoreHouse.UpdateEnable, L("Permission:UpdateEnable"));
            storeHousePerm.AddChild(ResourcePermissions.StoreHouse.Detail, L("Permission:Detail"));

            var terminalPerm = moduleGroup.AddPermission(ResourcePermissions.Terminal.Default, L("Permission:Terminal"));

            var terminalLinkPerm = moduleGroup.AddPermission(ResourcePermissions.TerminalLink.Default, L("Permission:TerminalLink"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ResourceResource>(name);
        }
    }
}