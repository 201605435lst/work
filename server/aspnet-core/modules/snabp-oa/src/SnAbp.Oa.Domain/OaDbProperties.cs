namespace SnAbp.Oa
{
    public static class OaDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.OaSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.OaSettings.DbSchema;

        public const string ConnectionStringName = "Oa";
    }
}
