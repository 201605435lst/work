namespace SnAbp.ComponentTrack
{
    public static class ComponentTrackDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Sn_ComponentTrack_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "ComponentTrack";
    }
}
