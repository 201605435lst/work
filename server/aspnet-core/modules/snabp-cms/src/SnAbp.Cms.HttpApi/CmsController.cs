using SnAbp.Cms.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Cms
{
    public abstract class CmsController : AbpController
    {
        protected CmsController()
        {
            LocalizationResource = typeof(CmsResource);
        }
    }
}
