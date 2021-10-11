using SnAbp.Material.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Material
{
    public abstract class MaterialController : AbpController
    {
        protected MaterialController()
        {
            LocalizationResource = typeof(MaterialResource);
        }
    }
}
