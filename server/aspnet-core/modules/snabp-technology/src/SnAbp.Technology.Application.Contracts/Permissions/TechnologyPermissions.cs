using Volo.Abp.Reflection;

namespace SnAbp.Technology.Permissions
{
    public class TechnologyPermissions
    {
        public const string GroupName = "AbpTechnology";
        public static class ConstructInterface
        {
            public const string Default = GroupName + ".ConstructInterfaces";
            public const string Reform = Default + ".Reform";
            public const string Import = Default + ".Import";
            public const string Sign = Default + ".Sign";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }
        public static class MaterialPlan
        {
            public const string Default = GroupName + ".MaterialPlans";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            public const string Approval = Default + ".Approval";
            public const string Delete = Default + ".Delete";
            public const string Submit = Default + ".Submit";
        }
        public static class Quanity
        {
            public const string Default = GroupName + ".Quanitys";
            public const string Statistic = Default + ".Statistic";
            public const string GenerateMaterialPlan = Default + ".GenerateMaterialPlan";
            public const string Export = Default + ".Export";

        }
        public static class ComponentRltQRCode
        {
            public const string Default = GroupName + ".ComponentRltQRCodes";
            public const string GenerateCode = Default + ".GenerateCode";
            public const string ExportCode = Default + ".ExportCode";
            public const string CodeDetail = Default + ".CodeDetail";
        }
        public static class Disclose
        {
            public const string Default = GroupName + ".Discloses";
            public const string Upload = Default + ".Upload";
            public const string Export = Default + ".Export";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(TechnologyPermissions));
        }
    }
}