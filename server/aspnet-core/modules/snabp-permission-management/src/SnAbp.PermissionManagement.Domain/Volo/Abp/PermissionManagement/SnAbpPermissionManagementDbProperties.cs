using Volo.Abp.Data;

namespace SnAbp.PermissionManagement
{
    public static class SnAbpPermissionManagementDbProperties
    {
       // public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;
        public static string DbTablePrefix { get; set; } = "Sn_App_";

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpPermissionManagement";
    }
}
