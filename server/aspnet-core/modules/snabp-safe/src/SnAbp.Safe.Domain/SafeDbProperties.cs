namespace SnAbp.Safe
{
    public static class SafeDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Safe_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Safe";
    }
}
