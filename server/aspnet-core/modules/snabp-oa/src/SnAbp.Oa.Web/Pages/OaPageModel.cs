using SnAbp.Oa.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace SnAbp.Oa.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class OaPageModel : AbpPageModel
    {
        protected OaPageModel()
        {
            LocalizationResourceType = typeof(OaResource);
            ObjectMapperContext = typeof(OaWebModule);
        }
    }
}