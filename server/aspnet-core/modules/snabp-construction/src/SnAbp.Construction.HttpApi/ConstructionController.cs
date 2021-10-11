using SnAbp.Construction.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Construction
{
    public abstract class ConstructionController : AbpController
    {
        protected ConstructionController()
        {
            LocalizationResource = typeof(ConstructionResource);
        }
    }
}
