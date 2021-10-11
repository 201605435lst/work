using Volo.Abp.Reflection;

namespace SnAbp.Project.Permissions
{
    public class ProjectPermissions
    {
        public const string GroupName = "AbpProject";

        public static class Project
        {
            public const string Default = GroupName + ".Project";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }
        public static class Dossier
        {
            public const string Default = GroupName + ".Dossier";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";
            public const string Download = Default + ".Download";

        }

        public static class ArchivesCategory
        {
            public const string Default = GroupName + ".ArchivesCategory";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Encrypt = Default + ".Encrypt";

        }
        public static class Archives
        {
            public const string Default = GroupName + ".Archives";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Apply = Default + ".Apply";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";

        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ProjectPermissions));
        }
    }
}