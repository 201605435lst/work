using SnAbp.Schedule.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Schedule
{
    public abstract class ScheduleAppService : ApplicationService
    {
        protected ScheduleAppService()
        {
            LocalizationResource = typeof(ScheduleResource);
            ObjectMapperContext = typeof(ScheduleApplicationModule);
        }
    }
}
