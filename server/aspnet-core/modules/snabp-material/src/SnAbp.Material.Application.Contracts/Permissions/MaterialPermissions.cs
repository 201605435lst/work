using Volo.Abp.Reflection;

namespace SnAbp.Material.Permissions
{
    public class MaterialPermissions
    {
        public const string GroupName = "AbpMaterial";

        public static class Supplier
        {
            public const string Default = GroupName + ".Suppliers";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }

        /// <summary>
        /// 库存管理
        /// </summary>
        public static class Inventory
        {
            public const string Default = GroupName + ".Inventories";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }

        /// <summary>
        /// 入库记录
        /// </summary>
        public static class EntryRecord
        {
            public const string Default = GroupName + ".EntryRecords";
            public const string Create = Default + ".Create";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }

        /// <summary>
        /// 出库记录
        /// </summary>
        public static class OutRecord
        {
            public const string Default = GroupName + ".OutRecords";
            public const string Create = Default + ".Create";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }

        /// <summary>
        /// 物资查询 -- 只有导出
        /// </summary>
        public static class Inquire
        {
            public const string Default = GroupName + ".Inquires";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }

        //public static class UsePlan
        //{
        //    public const string Default = GroupName + ".UsePlans";
        //    public const string Create = Default + ".Create";
        //    public const string Update = Default + ".Update";
        //    public const string Delete = Default + ".Delete";
        //    public const string Detail = Default + ".Detail";
        //    public const string Export = Default + ".Export";
        //    public const string Flow = Default + ".Flow";
        //    public const string Approval = Default + ".Approval";
        //}

        public static class PurchasePlan
        {
            public const string Default = GroupName + ".PurchasePlans";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            public const string Approval = Default + ".Approval";
        }

        public static class PurchaseList
        {
            public const string Default = GroupName + ".PurchaseLists";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            public const string Approval = Default + ".Approval";
        }

        public static class ConstructionSection
        {
            public const string Default = GroupName + ".ConstructionSections";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
        }
        public static class ConstructionTeam
        {
            public const string Default = GroupName + ".ConstructionTeams";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            public const string Import = Default + ".Import";

        }
        public static class MaterialAcceptance
        {
            public const string Default = GroupName + ".MaterialAcceptances";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }
        public static class MaterialOfBill
        {
            public const string Default = GroupName + ".MaterialOfBill";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
        }
        public static class MaterialPlan
        {
            public const string Default = GroupName + ".MaterialPlans";
            public const string GenerateMaterialPlan = Default + ".GenerateMaterialPlan";

        }
        public static class Material
        {
            public const string Default = GroupName + ".Materials";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string Export = Default + ".Export";
            public const string ExportCode = Default + ".ExportCode";
            public const string Synchronize = Default + ".Synchronize";
        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(MaterialPermissions));
        }
    }
}