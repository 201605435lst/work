using Volo.Abp.Reflection;

namespace SnAbp.Schedule.Permissions
{
    public class SchedulePermissions
    {
        public const string GroupName = "Schedule";

        public static class Schedule {
            public const string Default = GroupName + ".Schedule";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
            public const string SetFlow = Default + ".SetFlow";
        }
        public static class Approval
        {
            public const string Default = GroupName + ".Approval";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Export = Default + ".Export";
        }
        public static class Diary
        {
            public const string Default = GroupName + ".Diary";
            public const string Fill = Default + ".Fill";
            public const string Update = Default + ".Update";
            public const string View = Default + ".View";
            public const string PdfExport = Default + ".PdfExport";
            public const string ExcelExport = Default + ".ExcelExport";
            public const string LogStatistics = Default + ".LogStatistics";
            public const string Examination = Default + ".Examination";

            

        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(SchedulePermissions));
        }
    }
}