using Volo.Abp.Reflection;

namespace SnAbp.CostManagement.Permissions
{
    public class CostManagementPermissions
    {
        public const string GroupName = "AbpCostManagement";
        public static class CostOther
        {
            public const string Default = GroupName + ".CostOthers";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
        public static class PeopleCost
        {
            public const string Default = GroupName + ".PeopleCosts";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
        public static class MoneyList
        {
            public const string Default = GroupName + ".MoneyLists";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
        public static class Contract
        {
            public const string Default = GroupName + ".Contracts";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(CostManagementPermissions));
        }
    }
}