using Volo.Abp.Reflection;

namespace SnAbp.Report.Permissions
{
    public class ReportPermissions
    {
        public const string GroupName = "AbpReport";
        public static class Report
        {
            public const string Default = GroupName + ".Reports";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            //public const string ManagePermissions = Default + ".ManagePermissions";
        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ReportPermissions));
        }
    }
}