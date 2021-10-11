namespace SnAbp.Resource.Settings
{
    public static class ResourceSettings
    {
        public const string GroupName = "Resource";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */

        public const string DbTablePrefix = "Sn_Resource_";
        public const string DbSchema = null;


        #region 设备导入常量
        public const string DataImportRowFlag = "[SeenSun]";
        public const string Workshop = "[Workshop]";
        public const string Team = "[Team]";
        public const string Equip_Sub = "[Equip_Sub]";
        public const string UnitType = "[UnitType]";
        public const string Section = "[Section]";
        public const string Code = "[Code]";
        public const string Name = "[Name]";
        public const string SystemName = "[SystemName]";
        public const string Sta_RailwayId = "[Sta_RailwayId]";
        public const string Sta_StationId = "[Sta_StationId]";
        public const string Sta_MachineRoomId = "[Sta_MachineRoomId]";
        public const string Zone_RailwayId = "[Zone_RailwayId]";
        public const string Zone_KilometerMark = "[Zone_KilometerMark]";
        public const string Zone_MachineRoomId = "[Zone_MachineRoomId]";
        public const string Oth_RailwayId = "[Oth_RailwayId]";
        public const string Oth_MachineRoomId = "[Oth_MachineRoomId]";
        public const string MachineRoomCode = "[MachineRoomCode]"; 
        public const string MaintenanceOrgCode = "[MaintenanceOrgCode]";
        public const string Manufacture = "[Manufacture]";
        public const string StandardEquipmentSpec = "[StandardEquipmentSpec]";
        public const string UsedDate = "[UsedDate]";
        public const string RunningState = "[RunningState]";
        public const string UnusedDate = "[UnusedDate]";
        public const string ExpiredDate = "[ExpiredDate]";
        public const string Remark = "[Remark]";
        #endregion
    }
}