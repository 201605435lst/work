using Volo.Abp.Reflection;

namespace SnAbp.FileApprove.Permissions
{
    public class FileApprovePermissions
    {
        public const string GroupName = "FileApprove";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(FileApprovePermissions));
        }
    }
}