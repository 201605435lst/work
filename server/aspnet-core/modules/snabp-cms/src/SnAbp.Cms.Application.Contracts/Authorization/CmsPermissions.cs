using Volo.Abp.Reflection;

namespace SnAbp.Cms.Authorization
{
    public class CmsPermissions
    {

        public const string GroupName = "AbpCms";

        public static class Category
        {
            public const string Default = GroupName + ".Categories";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail"; 
            public const string UpdateEnable = Default + ".UpdateEnable";

        }

        public static class Article
        {
            public const string Default = GroupName + ".Articles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
        }
        
        public static class CategoryRltArticle
        {
            public const string Default = GroupName + ".CategoriesRltArticles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail";
            public const string UpdateOrder = Default + ".UpdateOrder";
            public const string UpdateEnable = Default + ".UpdateEnable";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(CmsPermissions));
        }
    }
}