using Volo.Abp.Application.Services;
using SnAbp.TenantManagement.Localization;

namespace SnAbp.TenantManagement
{
    public abstract class TenantManagementAppServiceBase : ApplicationService
    {
        protected TenantManagementAppServiceBase()
        {
            ObjectMapperContext = typeof(SnAbpTenantManagementApplicationModule);
            LocalizationResource = typeof(AbpTenantManagementResource);
        }
    }
}