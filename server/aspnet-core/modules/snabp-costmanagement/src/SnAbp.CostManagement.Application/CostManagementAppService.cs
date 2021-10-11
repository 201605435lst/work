using SnAbp.CostManagement.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.CostManagement
{
    public abstract class CostManagementAppService : ApplicationService
    {
        protected CostManagementAppService()
        {
            LocalizationResource = typeof(CostManagementResource);
            ObjectMapperContext = typeof(CostManagementApplicationModule);
        }
    }
}
