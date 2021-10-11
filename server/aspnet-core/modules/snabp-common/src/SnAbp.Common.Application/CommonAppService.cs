using SnAbp.Common.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Common
{
    public abstract class CommonAppService : ApplicationService
    {
        protected CommonAppService()
        {
            LocalizationResource = typeof(CommonResource);
            ObjectMapperContext = typeof(CommonApplicationModule);
        }
    }
} 
