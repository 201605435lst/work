namespace SnAbp.CostManagement
{
    public static class CostManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_CostManagement_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "CostManagement";
    }
}
