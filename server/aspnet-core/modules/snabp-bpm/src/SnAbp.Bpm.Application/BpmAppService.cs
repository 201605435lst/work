using SnAbp.Bpm.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Bpm
{
    public abstract class BpmAppService : ApplicationService
    {
        protected BpmAppService()
        {
            LocalizationResource = typeof(BpmResource);
            ObjectMapperContext = typeof(BpmApplicationModule);
        }
    }
} 
