namespace SnAbp.StdBasic
{
    public static class StdBasicDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.StdBasicSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.StdBasicSettings.DbSchema;

        public const string ConnectionStringName = "StdBasic";
    }
}
