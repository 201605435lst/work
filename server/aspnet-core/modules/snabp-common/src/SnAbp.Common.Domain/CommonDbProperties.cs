namespace SnAbp.Common
{
    public static class CommonDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.CommonSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.CommonSettings.DbSchema;

        public const string ConnectionStringName = "Common";
    }
}
