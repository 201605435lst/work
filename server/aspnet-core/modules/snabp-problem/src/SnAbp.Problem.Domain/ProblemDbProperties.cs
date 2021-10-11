namespace SnAbp.Problem
{
    public static class ProblemDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.ProblemSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.ProblemSettings.DbSchema;

        public const string ConnectionStringName = "Problem";
    }
}
