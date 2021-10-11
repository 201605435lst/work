using SnAbp.Alarm.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Alarm
{
    public abstract class AlarmController : AbpController
    {
        protected AlarmController()
        {
            LocalizationResource = typeof(AlarmResource);
        }
    }
}
