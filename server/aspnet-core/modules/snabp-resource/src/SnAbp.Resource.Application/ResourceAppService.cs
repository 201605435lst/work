using SnAbp.Resource.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource
{
    public abstract class ResourceAppService : ApplicationService
    {
        protected ResourceAppService()
        {
            LocalizationResource = typeof(ResourceResource);
            ObjectMapperContext = typeof(ResourceApplicationModule);
        }
    }
} 
