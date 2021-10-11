namespace SnAbp.Message
{
    public static class MessageDbProperties
    {
        public const string ConnectionStringName = "Message";
        public static string DbTablePrefix { get; set; } = "Sn_Message_";

        public static string DbSchema { get; set; } = null;
    }
}