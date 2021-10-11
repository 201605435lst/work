using Volo.Abp.Reflection;

namespace SnAbp.Alarm.Permissions
{
    public class AlarmPermissions
    {
        public const string GroupName = "Alarm";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(AlarmPermissions));
        }
    }
}