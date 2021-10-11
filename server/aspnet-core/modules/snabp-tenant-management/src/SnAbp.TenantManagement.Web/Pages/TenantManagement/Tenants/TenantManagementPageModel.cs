using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace SnAbp.TenantManagement.Web.Pages.TenantManagement.Tenants
{
    public abstract class TenantManagementPageModel : AbpPageModel
    {
        public TenantManagementPageModel()
        {
            ObjectMapperContext = typeof(SnAbpTenantManagementWebModule);
        }
    }
}