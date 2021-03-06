using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace SnAbp.Identity.Web.Pages.Identity
{
    public abstract class IdentityPageModel : AbpPageModel
    {
        protected IdentityPageModel()
        {
            ObjectMapperContext = typeof(SnAbpIdentityWebModule);
        }
    }
}