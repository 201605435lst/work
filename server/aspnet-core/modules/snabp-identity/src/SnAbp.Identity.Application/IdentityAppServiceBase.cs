using Volo.Abp.Application.Services;
using SnAbp.Identity.Localization;

namespace SnAbp.Identity
{
    public abstract class IdentityAppServiceBase : ApplicationService
    {
        protected IdentityAppServiceBase()
        {
            ObjectMapperContext = typeof(SnAbpIdentityApplicationModule);
            LocalizationResource = typeof(IdentityResource);
        }
    }
}