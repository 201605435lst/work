using Volo.Abp.Application.Services;
using SnAbp.FeatureManagement.Localization;

namespace SnAbp.FeatureManagement
{
    public abstract class FeatureManagementAppServiceBase : ApplicationService
    {
        protected FeatureManagementAppServiceBase()
        {
            ObjectMapperContext = typeof(SnAbpFeatureManagementApplicationModule);
            LocalizationResource = typeof(AbpFeatureManagementResource);
        }
    }
}