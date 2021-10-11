using SnAbp.Technology.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Technology
{
    public abstract class TechnologyController : AbpController
    {
        protected TechnologyController()
        {
            LocalizationResource = typeof(TechnologyResource);
        }
    }
}
