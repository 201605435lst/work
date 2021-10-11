using Volo.Abp.Data;

namespace SnAbp.TenantManagement
{
    public static class SnAbpTenantManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_App_";

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpTenantManagement";
    }
}
