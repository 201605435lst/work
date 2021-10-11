namespace SnAbp.Regulation
{
    public static class RegulationDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Regulation_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Regulation";
    }
}
