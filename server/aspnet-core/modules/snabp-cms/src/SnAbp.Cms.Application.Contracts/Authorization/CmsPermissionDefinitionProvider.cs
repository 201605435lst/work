using SnAbp.Cms.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Cms.Authorization
{
    public class CmsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {

            var moduleGroup = context.AddGroup(CmsPermissions.GroupName, L(CmsPermissions.GroupName));


            var categoryPerm = moduleGroup.AddPermission(CmsPermissions.Category.Default, L("Permission:Categories"));
            categoryPerm.AddChild(CmsPermissions.Category.Create, L("Permission:Create"));
            categoryPerm.AddChild(CmsPermissions.Category.Delete, L("Permission:Delete"));
            categoryPerm.AddChild(CmsPermissions.Category.Update, L("Permission:Update"));
            categoryPerm.AddChild(CmsPermissions.Category.Detail, L("Permission:Detail"));
            categoryPerm.AddChild(CmsPermissions.Category.UpdateEnable, L("Permission:UpdateEnable")); 


             var articlePerm = moduleGroup.AddPermission(CmsPermissions.Article.Default, L("Permission:Articles"));
            articlePerm.AddChild(CmsPermissions.Article.Create, L("Permission:Create"));
            articlePerm.AddChild(CmsPermissions.Article.Delete, L("Permission:Delete"));
            articlePerm.AddChild(CmsPermissions.Article.Update, L("Permission:Update"));
            articlePerm.AddChild(CmsPermissions.Article.Detail, L("Permission:Detail"));

            var categoryRltArticlePerm = moduleGroup.AddPermission(CmsPermissions.CategoryRltArticle.Default, L("Permission:CategoriesRltArticles"));
            categoryRltArticlePerm.AddChild(CmsPermissions.CategoryRltArticle.Create, L("Permission:Create"));
            categoryRltArticlePerm.AddChild(CmsPermissions.CategoryRltArticle.Delete, L("Permission:Delete"));
            categoryRltArticlePerm.AddChild(CmsPermissions.CategoryRltArticle.Update, L("Permission:Update"));
            categoryRltArticlePerm.AddChild(CmsPermissions.CategoryRltArticle.Detail, L("Permission:Detail"));
            categoryRltArticlePerm.AddChild(CmsPermissions.CategoryRltArticle.UpdateOrder, L("Permission:UpdateOrder"));
            categoryRltArticlePerm.AddChild(CmsPermissions.CategoryRltArticle.UpdateEnable, L("Permission:UpdateEnable"));


        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CmsResource>(name);
        }
    }
}