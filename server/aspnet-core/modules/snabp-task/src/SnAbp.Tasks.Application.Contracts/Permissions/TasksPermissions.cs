using Volo.Abp.Reflection;

namespace SnAbp.Tasks.Permissions
{
    public class TasksPermissions
    {
        public const string GroupName = "Tasks";

        public static class Task
        {
            public const string Default = GroupName + ".Task";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(TasksPermissions));
        }
    }
}