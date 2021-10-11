using Volo.Abp.Reflection;

namespace SnAbp.Basic.Authorization
{
    public class BasicPermissions
    {

        public const string GroupName = "AbpBasic";

        public static class Railway
        {
            public const string Default = GroupName + ".Railways";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Relate = Default + ".Relate";
            public const string Detail = Default + ".Detail";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class Station
        {
            public const string Default = GroupName + ".Stations";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class InstallationSite
        {
            public const string Default = GroupName + ".InstallationSites";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        //public const string Basic_Railway = "Basic.Railway";
        //public const string Basic_Railway_Create = "Basic.Railway.Create";
        //public const string Basic_Railway_Update = "Basic.Railway.Update";
        //public const string Basic_Railway_Relate = "Basic.Railway.Relate";
        //public const string Basic_Railway_Delete = "Basic.Railway.Delete";

        //public const string Basic_Station = "Basic.Station";
        //public const string Basic_Station_Create = "Basic.Station.Create";
        //public const string Basic_Station_Update = "Basic.Station.Update";
        //public const string Basic_Station_Delete = "Basic.Station.Delete";

        //public const string Basic_InstallationSite = "Basic.InstallationSite";
        //public const string Basic_InstallationSite_Create = "Basic.InstallationSite.Create";
        //public const string Basic_InstallationSite_Update = "Basic.InstallationSite.Update";
        //public const string Basic_InstallationSite_Delete = "Basic.InstallationSite.Delete";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BasicPermissions));
        }
    }
}