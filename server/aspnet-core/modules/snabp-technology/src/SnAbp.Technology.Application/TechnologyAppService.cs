using SnAbp.Technology.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Technology
{
    public abstract class TechnologyAppService : ApplicationService
    {
        protected TechnologyAppService()
        {
            LocalizationResource = typeof(TechnologyResource);
            ObjectMapperContext = typeof(TechnologyApplicationModule);
        }
    }
}
