namespace SnAbp.Emerg
{
    public static class EmergDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.EmergSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.EmergSettings.DbSchema;

        public const string ConnectionStringName = "Emerg";
    }
}
