using Volo.Abp.Reflection;

namespace SnAbp.Oa.Permissions
{
    public class OaPermissions
    {
        public const string GroupName = "AbpOa";

        
        public static class DutySchedule
        {
            public const string Default = GroupName + ".DutySchedule";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            //public const string ManagePermissions = Default + ".ManagePermissions";
        }
        public static class Contract
        {
            public const string Default = GroupName + ".Contracts";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Apply = Default + ".Apply";
            public const string Export = Default + ".Export";
            //public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class Seal
        {
            public const string Default = GroupName + ".Seals";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
            public const string Lock = Default + ".Lock";
            public const string Efficiency = Default + ".Efficiency";
            public const string RestPSW = Default + ".RestPSW";
            //public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(OaPermissions));
        }
    }
}