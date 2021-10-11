using SnAbp.Quality.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Quality
{
    public abstract class QualityAppService : ApplicationService
    {
        protected QualityAppService()
        {
            LocalizationResource = typeof(QualityResource);
            ObjectMapperContext = typeof(QualityApplicationModule);
        }
    }
}
