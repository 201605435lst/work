using Volo.Abp.Reflection;

namespace SnAbp.Regulation.Permissions
{
    public class RegulationPermissions
    {
        public const string GroupName = "AbpRegulation";


        public static class Institution
        {
            public const string Default = GroupName + ".Institutions";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
            public const string Authority = Default + ".Authority";
            public const string Audit = Default + ".Audit";
        }

        public static class Label
        {
            public const string Default = GroupName + ".Labels";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(RegulationPermissions));
        }


    }
}