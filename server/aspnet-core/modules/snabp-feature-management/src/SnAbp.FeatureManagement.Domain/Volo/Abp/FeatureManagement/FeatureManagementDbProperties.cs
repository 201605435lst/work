using Volo.Abp.Data;

namespace SnAbp.FeatureManagement
{
    public static class FeatureManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_App_";

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpFeatureManagement";
    }
}
