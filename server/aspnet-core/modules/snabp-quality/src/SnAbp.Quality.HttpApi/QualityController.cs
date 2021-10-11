using SnAbp.Quality.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Quality
{
    public abstract class QualityController : AbpController
    {
        protected QualityController()
        {
            LocalizationResource = typeof(QualityResource);
        }
    }
}
