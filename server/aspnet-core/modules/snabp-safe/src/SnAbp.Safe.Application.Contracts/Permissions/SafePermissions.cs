using Volo.Abp.Reflection;

namespace SnAbp.Safe.Permissions
{
    public class SafePermissions
    {
        public const string GroupName = "AbpSafe";
        public static class SafeProblem
        {
            public const string Default = GroupName + ".Problems";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            public const string Reform = Default + ".Reform";
            public const string Update = Default + ".Update";
            public const string Sign = Default + ".Sign"; 
            public const string Verify = Default + ".Verify";
            public const string Position = Default + ".Position";
        }
        public static class SafeProblemLibrary
        {
            public const string Default = GroupName + ".ProblemLibrarys";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";
            public const string Delete = Default + ".Delete"; 
        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(SafePermissions));
        }
    }
}