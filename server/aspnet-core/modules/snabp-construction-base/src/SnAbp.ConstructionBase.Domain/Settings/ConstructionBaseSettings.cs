namespace SnAbp.ConstructionBase.Settings
{
    public static class ConstructionBaseSettings
    {
        public const string GroupName = "ConstructionBase";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */
        
        public static string DbTablePrefix { get; set; } = "Sn_ConstructionBase_";

        public static string DbSchema { get; set; } = null;
    }
}