using Volo.Abp.Data;

namespace SnAbp.SettingManagement
{
    public static class AbpSettingManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_App_";

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpSettingManagement";
    }
}
