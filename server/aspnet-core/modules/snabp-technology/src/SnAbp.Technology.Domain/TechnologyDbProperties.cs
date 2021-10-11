namespace SnAbp.Technology
{
    public static class TechnologyDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Technology_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Technology";
    }
}
