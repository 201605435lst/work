namespace SnAbp.Bpm
{
    public static class BpmDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.BpmSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.BpmSettings.DbSchema;

        public const string ConnectionStringName = "Bpm";
    }
}
