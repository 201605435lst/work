using SnAbp.Cms.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Cms
{
    public abstract class CmsAppService : ApplicationService
    {
        protected CmsAppService()
        {
            LocalizationResource = typeof(CmsResource);
            ObjectMapperContext = typeof(CmsApplicationModule);
        }
    }
} 
