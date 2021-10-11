namespace SnAbp.StdBasic.Settings
{
    public static class StdBasicSettings
    {
        public const string GroupName = "StdBasic";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */

        public const string DbTablePrefix = "Sn_StdBasic_";
        public const string DbSchema = null;

        #region 构件及产品导入常量
        public const string DataImportRowFlag = "[SeenSun]";
        public const string Code = "[Code]";
        public const string Name = "[Name]";
        public const string ProductCategories = "[ProductCategories]";
        public const string ExtendCode = "[ExtendCode]";
        public const string ExtendName = "[ExtendName]";
        public const string LevelName = "[LevelName]";
        public const string Unit = "[Unit]";
        public const string Remark = "[Remark]";
        #endregion
    }
}