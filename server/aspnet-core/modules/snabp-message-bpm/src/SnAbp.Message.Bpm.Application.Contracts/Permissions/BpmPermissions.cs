using Volo.Abp.Reflection;

namespace SnAbp.Message.Bpm.Permissions
{
    public class BpmPermissions
    {
        public const string GroupName = "Bpm";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BpmPermissions));
        }
    }
}