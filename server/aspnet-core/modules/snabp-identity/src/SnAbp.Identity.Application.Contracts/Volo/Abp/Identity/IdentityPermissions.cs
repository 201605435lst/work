using Volo.Abp.Reflection;

namespace SnAbp.Identity
{
    public static class IdentityPermissions
    {
        public const string GroupName = "AbpIdentity";

        //角色
        public static class Roles
        {
            public const string Default = GroupName + ".Roles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete"; 
            //public const string Authorization = Default + ".Authorization";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        //用户
        public static class Users
        {
            public const string Default = GroupName + ".Users";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
            public const string AssignRoles = Default + ".AssignRoles";
            public const string Detail = Default + ".Detail";
            public const string Reset = Default + ".Reset";
            //public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class UserLookup
        {
            public const string Default = GroupName + ".UserLookup";
        }

        //组织单元
        public  static class Organization
        {
            public const string Default = GroupName + ".Organization";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
            //public const string ManagePermissions = Default + ".ManagePermissions";
        }

        //数据字典
        public static class DataDictionary
        {
            public const string Default = GroupName + ".DataDictionary";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            //public const string ManagePermissions = Default + ".ManagePermissions";
        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityPermissions));
        }
    }
}