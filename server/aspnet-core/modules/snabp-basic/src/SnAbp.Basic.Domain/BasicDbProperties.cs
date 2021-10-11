namespace SnAbp.Basic
{
    public static class BasicDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.BasicSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.BasicSettings.DbSchema;

        public const string ConnectionStringName = "Basic";
    }
}
