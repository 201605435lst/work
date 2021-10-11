using SnAbp.Report.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Report
{
    public abstract class ReportAppService : ApplicationService
    {
        protected ReportAppService()
        {
            LocalizationResource = typeof(ReportResource);
            ObjectMapperContext = typeof(ReportApplicationModule);
        }
    }
}
