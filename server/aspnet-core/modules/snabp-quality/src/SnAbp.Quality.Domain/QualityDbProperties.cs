namespace SnAbp.Quality
{
    public static class QualityDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Quality_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Quality";
    }
}
