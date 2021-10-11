using SnAbp.StdBasic.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic
{
    public abstract class StdBasicAppService : ApplicationService
    {
        protected StdBasicAppService()
        {
            LocalizationResource = typeof(StdBasicResource);
            ObjectMapperContext = typeof(StdBasicApplicationModule);
        }
    }
} 
