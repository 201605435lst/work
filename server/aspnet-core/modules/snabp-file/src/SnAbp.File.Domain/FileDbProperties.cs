using SnAbp.File.Settings;

namespace SnAbp.File
{
    public static class FileDbProperties
    {
        public const string ConnectionStringName = "File";
        public static string DbTablePrefix { get; set; } = FileSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = FileSettings.DbSchema;
    }
}