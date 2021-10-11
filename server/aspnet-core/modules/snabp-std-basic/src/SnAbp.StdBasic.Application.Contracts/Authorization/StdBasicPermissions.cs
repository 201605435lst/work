using Volo.Abp.Reflection;

namespace SnAbp.StdBasic.Authorization
{
    public class StdBasicPermissions
    {
        public const string GroupName = "AbpStdBasic";

        public static class ComponentCategory
        {
            public const string Default = GroupName + ".ComponentCategories";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class ProductCategory
        {
            public const string Default = GroupName + ".ProductCategories";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class Manufacture
        {
            public const string Default = GroupName + ".Manufactures";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }


        public static class StandardEquipment
        {
            public const string Default = GroupName + ".StandardEquipments";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class Terminal
        {
            public const string Default = GroupName + ".Terminals";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }

        public static class RepairGroup
        {
            public const string Default = GroupName + ".RepairGroup";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }
        public static class WorkAttention
        {
            public const string Default = GroupName + ".WorkAttention";
            public const string Create = Default + ".Create";
            public const string CreateType = Default + ".CreateType";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }
        public static class RepairItem
        {
            public const string Default = GroupName + ".RepairItems";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail"; 
            public const string Delete = Default + ".Delete";
            public const string CreateTagMigration = Default + ".CreateTagMigration";

        }

        public static class RepairTestItem
        {
            public const string Default = GroupName + ".RepairTestItems";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete"; 
            public const string Upgrade = Default + ".Upgrade";
            public const string UpdateOrder = Default + ".UpdateOrder";

        }



        public static class InfluenceRange
        {
            public const string Default = GroupName + ".InfluenceRanges";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }


        public static class MVDCategory
        {
            public const string Default = GroupName + ".MVDCategory";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class MVDProperty
        {
            public const string Default = GroupName + ".MVDProperty";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class IndividualProject
        {
            public const string Default = GroupName + ".IndividualProjects";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class ProcessTemplate
        {
            public const string Default = GroupName + ".ProcessTemplates";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }
        public static class ProjectItem
        {
            public const string Default = GroupName + ".ProjectItems";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class QuotaCategory
        {
            public const string Default = GroupName + ".QuotaCategorys";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }
        public static class Quota
        {
            public const string Default = GroupName + ".Quotas";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class QuotaItem
        {
            public const string Default = GroupName + ".QuotaItems";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static class BasePrice
        {
            public const string Default = GroupName + ".BasePrices";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }
        public static class ComputerCode
        {
            public const string Default = GroupName + ".ComputerCodes";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
            public const string Import = Default + ".Import";
            public const string Export = Default + ".Export";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(StdBasicPermissions));
        }
    }
}