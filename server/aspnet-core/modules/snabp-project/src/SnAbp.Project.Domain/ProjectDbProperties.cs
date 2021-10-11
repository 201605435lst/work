namespace SnAbp.Project
{
    public static class ProjectDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_Project_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Project";
    }
}
