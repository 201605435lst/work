using SnAbp.Regulation.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Regulation
{
    public abstract class RegulationAppService : ApplicationService
    {
        protected RegulationAppService()
        {
            LocalizationResource = typeof(RegulationResource);
            ObjectMapperContext = typeof(RegulationApplicationModule);
        }
    }
}
