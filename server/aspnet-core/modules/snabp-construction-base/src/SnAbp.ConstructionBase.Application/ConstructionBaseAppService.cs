using SnAbp.ConstructionBase.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.ConstructionBase
{
    public abstract class ConstructionBaseAppService : ApplicationService
    {
        protected ConstructionBaseAppService()
        {
            LocalizationResource = typeof(ConstructionBaseResource);
            ObjectMapperContext = typeof(ConstructionBaseApplicationModule);
        }
    }
}
