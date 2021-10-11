using SnAbp.Resource.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Resource
{
    public abstract class ResourceController : AbpController
    {
        protected ResourceController()
        {
            LocalizationResource = typeof(ResourceResource);
        }
    }
}
