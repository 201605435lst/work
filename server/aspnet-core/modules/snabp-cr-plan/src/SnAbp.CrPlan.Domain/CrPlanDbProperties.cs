namespace SnAbp.CrPlan
{
    public static class CrPlanDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.CrPlanSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.CrPlanSettings.DbSchema;

        public const string ConnectionStringName = "CrPlan";
    }
}
