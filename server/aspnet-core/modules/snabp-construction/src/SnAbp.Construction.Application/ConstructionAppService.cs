using SnAbp.Construction.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Construction
{
    public abstract class ConstructionAppService : ApplicationService
    {
        protected ConstructionAppService()
        {
            LocalizationResource = typeof(ConstructionResource);
            ObjectMapperContext = typeof(ConstructionApplicationModule);
        }
    }
}
