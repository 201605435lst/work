using Volo.Abp.Reflection;

namespace SnAbp.Quality.Permissions
{
    public class QualityPermissions
    {
        public const string GroupName = "AbpQuality";
        public static class QualityProblems
        {
            public const string Default = GroupName + ".QualityProblems";
            public const string Create = Default + ".Create";
            public const string Improve = Default + ".Improve";
            public const string Verify = Default + ".Verify";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            public const string Update = Default + ".Update";
            public const string Position = Default + ".Position";
        }

        public static class QualityProblemLibraries
        {
            public const string Default = GroupName + ".QualityProblemLibraries";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";
           

        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(QualityPermissions));
        }
    }
}