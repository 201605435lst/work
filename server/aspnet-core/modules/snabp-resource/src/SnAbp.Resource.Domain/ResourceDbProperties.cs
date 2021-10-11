namespace SnAbp.Resource
{
    public static class ResourceDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.ResourceSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.ResourceSettings.DbSchema;

        public const string ConnectionStringName = "Resource";
    }
}
