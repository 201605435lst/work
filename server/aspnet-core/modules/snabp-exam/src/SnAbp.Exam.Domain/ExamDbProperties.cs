namespace SnAbp.Exam
{
    public static class ExamDbProperties
    {
        public static string DbTablePrefix { get; set; } = Settings.ExamSettings.DbTablePrefix;

        public static string DbSchema { get; set; } = Settings.ExamSettings.DbSchema;

        public const string ConnectionStringName = "Exam";
    }
}
