namespace SnAbp.Basic.Settings
{
    public static class BasicSettings
    {
        public const string GroupName = "Basic";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */

        public const string DbTablePrefix = "Sn_Basic_";
        public const string DbSchema = null;

        #region ���ҵ��볣��
        public const string DataImportRowFlag = "[SeenSun]";
        public const string Name = "[Name]";
        public const string CSRGCode = "[CSRGCode]";
        public const string ShortName = "[ShortName]";
        public const string Address = "[Address]";
        public const string Telephone = "[Telephone]";
        #endregion

        #region ͨ��
        public const string Type = "[Type]";
        #endregion

        #region ��·���볣��
        public const string Organization = "[Organization]";
        public const string DownLink = "[DownLink]";
        public const string UpLink = "[UpLink]";
        #endregion

        #region վ�㵼�볣��
        public const string RailwayName = "[RailwayName]";
        public const string KilometerMark = "[KilometerMark]";
        public const string DownLinkKMMark = "[DownLinkKMMark]";
        #endregion
    }
}