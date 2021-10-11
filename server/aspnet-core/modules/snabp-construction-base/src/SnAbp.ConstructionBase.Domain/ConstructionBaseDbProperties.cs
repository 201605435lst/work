namespace SnAbp.ConstructionBase
{
    public static class ConstructionBaseDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_ConstructionBase_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "ConstructionBase";
    }
}
