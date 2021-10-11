namespace SnAbp.Construction
{
    public static class ConstructionDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Construction_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Construction";
    }
}
