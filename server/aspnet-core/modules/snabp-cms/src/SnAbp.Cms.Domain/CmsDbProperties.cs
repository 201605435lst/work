namespace SnAbp.Cms
{
    public static class CmsDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.CmsSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.CmsSettings.DbSchema;

        public const string ConnectionStringName = "Cms";
    }
}
