namespace SnAbp.Schedule
{
    public static class ScheduleDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Schedule_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Schedule";
    }
}
