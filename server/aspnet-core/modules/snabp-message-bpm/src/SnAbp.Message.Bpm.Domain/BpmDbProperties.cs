namespace SnAbp.Message.Bpm
{
    public static class BpmDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Message_Bpm_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Message_Bpm";
    }
}
