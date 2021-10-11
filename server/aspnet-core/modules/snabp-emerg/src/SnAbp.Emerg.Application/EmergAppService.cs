using SnAbp.Emerg.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Emerg
{
    public abstract class EmergAppService : ApplicationService
    {
        protected EmergAppService()
        {
            LocalizationResource = typeof(EmergResource);
            ObjectMapperContext = typeof(EmergApplicationModule);
        }
    }
} 
