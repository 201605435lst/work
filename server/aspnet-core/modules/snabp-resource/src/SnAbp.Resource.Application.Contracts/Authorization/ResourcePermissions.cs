using Volo.Abp.Reflection;

namespace SnAbp.Resource.Authorization
{
    public class ResourcePermissions
    {
        public const string GroupName = "AbpResource";

        public static class CableExtend
        {
            public const string Default = GroupName + ".CableExtends";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
        }

        public static class CableLocation
        {
            public const string Default = GroupName + ".CableLocations";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }

        public static class Equipment
        {
            public const string Default = GroupName + ".Equipments";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
            public const string GenerateQuality = Default + ".GenerateQuality";
            public const string WaitingStorage = Default + ".WaitingStorage";
        }

        public static class EquipmentGroup
        {
            public const string Default = GroupName + ".EquipmentGroups";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class EquipmentProperty
        {
            public const string Default = GroupName + ".EquipmentProperties";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class StoreEquipment
        {
            public const string Default = GroupName + ".StoreEquipments";
            public const string Create = Default + ".Create";
            public const string Export = Default + ".Export";
        }

        public static class StoreEquipmentTest
        {
            public const string Default = GroupName + ".StoreEquipmentTest";
            public const string Create = Default + ".Create";
            public const string Detail = Default + ".Detail";
        }

        public static class StoreEquipmentTransfer
        {
            public const string Default = GroupName + ".StoreEquipmentTransfer";
            public const string Create = Default + ".Create";
            public const string Detail = Default + ".Detail";
        }

        public static class StoreHouse
        {
            public const string Default = GroupName + ".StoreHouse";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string UpdateEnable = Default + ".UpdateEnable";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
        }

        public static class Terminal
        {
            public const string Default = GroupName + ".Terminal";
        }

        public static class TerminalLink
        {
            public const string Default = GroupName + ".TerminalLink";
        }



        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ResourcePermissions));
        }
    }
}