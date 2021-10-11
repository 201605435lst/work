namespace SnAbp.Message.Notice
{
    public static class NoticeDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Message_Notice_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Notice";
    }
}
