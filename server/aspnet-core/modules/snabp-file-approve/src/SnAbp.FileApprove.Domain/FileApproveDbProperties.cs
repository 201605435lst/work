namespace SnAbp.FileApprove
{
    public static class FileApproveDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_FileApprove_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "FileApprove";
    }
}
