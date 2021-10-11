namespace SnAbp.IdentityServer
{
    public static class SnAbpIdentityServerDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_App_IdentityServer";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "SnAbpIdentityServer";
    }
}
