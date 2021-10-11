using SnAbp.Report.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Report
{
    public abstract class ReportController : AbpController
    {
        protected ReportController()
        {
            LocalizationResource = typeof(ReportResource);
        }
    }
}
