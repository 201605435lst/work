using SnAbp.CrPlan.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.CrPlan
{
    public abstract class CrPlanController : AbpController
    {
        protected CrPlanController()
        {
            LocalizationResource = typeof(CrPlanResource);
        }
    }
}
