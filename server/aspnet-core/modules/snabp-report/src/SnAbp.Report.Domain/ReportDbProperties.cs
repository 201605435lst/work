namespace SnAbp.Report
{
    public static class ReportDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Report_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Report";
    }
}
