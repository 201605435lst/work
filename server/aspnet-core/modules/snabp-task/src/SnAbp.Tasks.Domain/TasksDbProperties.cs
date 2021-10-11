namespace SnAbp.Tasks
{
    public static class TasksDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Task_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Task";
    }
}
