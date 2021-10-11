using SnAbp.ConstructionBase.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.ConstructionBase
{
    public abstract class ConstructionBaseController : AbpController
    {
        protected ConstructionBaseController()
        {
            LocalizationResource = typeof(ConstructionBaseResource);
        }
    }
}
