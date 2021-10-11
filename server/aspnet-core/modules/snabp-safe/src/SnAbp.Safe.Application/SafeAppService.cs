using SnAbp.Safe.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Safe
{
    public abstract class SafeAppService : ApplicationService
    {
        protected SafeAppService()
        {
            LocalizationResource = typeof(SafeResource);
            ObjectMapperContext = typeof(SafeApplicationModule);
        }
    }
}
