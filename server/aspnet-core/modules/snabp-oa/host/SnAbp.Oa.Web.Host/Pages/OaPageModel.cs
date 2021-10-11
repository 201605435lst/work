using SnAbp.Oa.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace SnAbp.Oa.Pages
{
    public abstract class OaPageModel : AbpPageModel
    {
        protected OaPageModel()
        {
            LocalizationResourceType = typeof(OaResource);
        }
    }
}