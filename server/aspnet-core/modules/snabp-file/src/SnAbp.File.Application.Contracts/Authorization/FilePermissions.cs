using Volo.Abp.Reflection;

namespace SnAbp.File.Authorization
{
    public class FilePermissions
    {
        public const string GroupName = "AbpFile";

        public static class File
        {
            public const string Default = GroupName + ".File";
            //public const string Create = Default + ".Create";
            //public const string Update = Default + ".Update";
            //public const string Delete = Default + ".Delete";
            //public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class FileManager
        {
            public const string Default = GroupName + ".FileManager";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class Folder
        {
            public const string Default = GroupName + ".Folder";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class OssConfig
        {
            public const string Default = GroupName + ".OssConfig";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Enable = Default + ".Enable";
            //public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class Tag
        {
            public const string Default = GroupName + ".Tag";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(FilePermissions));
        }
    }
}