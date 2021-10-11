namespace SnAbp.Material
{
    public static class MaterialDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.MaterialSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.MaterialSettings.DbSchema;

        public const string ConnectionStringName = "Material";
    }
}
