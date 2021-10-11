namespace SnAbp.Message.IOT
{
    public static class IOTDbProperties
    {
        public static string DbTablePrefix { get; set; } = "IOT";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "IOT";
    }
}
