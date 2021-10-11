using SnAbp.CrPlan.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.CrPlan
{
    public abstract class CrPlanAppService : ApplicationService
    {
        protected CrPlanAppService()
        {
            LocalizationResource = typeof(CrPlanResource);
            ObjectMapperContext = typeof(CrPlanApplicationModule);
        }
    }
} 
