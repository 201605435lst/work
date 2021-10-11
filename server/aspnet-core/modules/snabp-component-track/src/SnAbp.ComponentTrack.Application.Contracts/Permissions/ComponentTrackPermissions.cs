using Volo.Abp.Reflection;

namespace SnAbp.ComponentTrack.Permissions
{
    public class ComponentTrackPermissions
    {
        public const string GroupName = "AbpComponentTrack";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ComponentTrackPermissions));
        }
    }
}