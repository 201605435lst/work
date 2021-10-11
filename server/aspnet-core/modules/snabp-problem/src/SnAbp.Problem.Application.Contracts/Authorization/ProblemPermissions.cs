using Volo.Abp.Reflection;

namespace SnAbp.Problem.Authorization
{
    public class ProblemPermissions
    {

        public const string GroupName = "AbpProblem";

        public static class Problem
        {
            public const string Default = GroupName + ".Problems";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }

        public static class ProblemCategory
        {
            public const string Default = GroupName + ".ProblemCategories";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ProblemPermissions));
        }
    }
}