using SnAbp.Regulation.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Regulation
{
    public abstract class RegulationController : AbpController
    {
        protected RegulationController()
        {
            LocalizationResource = typeof(RegulationResource);
        }
    }
}
