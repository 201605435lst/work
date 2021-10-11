namespace SnAbp.Alarm
{
    public static class AlarmDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Alarm_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Alarm";
    }
}
