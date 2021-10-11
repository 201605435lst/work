using SnAbp.Schedule.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Schedule
{
    public abstract class ScheduleController : AbpController
    {
        protected ScheduleController()
        {
            LocalizationResource = typeof(ScheduleResource);
        }
    }
}
