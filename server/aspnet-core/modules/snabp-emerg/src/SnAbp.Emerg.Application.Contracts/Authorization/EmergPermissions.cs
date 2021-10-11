using Volo.Abp.Reflection;

namespace SnAbp.Emerg.Authorization
{
    public class EmergPermissions
    {

        public const string GroupName = "AbpEmerg";

        public static class Fault
        {
            public const string Default = GroupName + ".Faults";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
           
        }

        public static class Plan
        {
            public const string Default = GroupName + ".Plans";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Apply = Default + ".Apply";
        }


        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(EmergPermissions));
        }
    }
}