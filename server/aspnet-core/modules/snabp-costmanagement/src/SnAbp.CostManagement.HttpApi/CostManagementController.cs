using SnAbp.CostManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.CostManagement
{
    public abstract class CostManagementController : AbpController
    {
        protected CostManagementController()
        {
            LocalizationResource = typeof(CostManagementResource);
        }
    }
}
