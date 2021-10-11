using SnAbp.Alarm.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Alarm
{
    public abstract class AlarmAppService : ApplicationService
    {
        protected AlarmAppService()
        {
            LocalizationResource = typeof(AlarmResource);
            ObjectMapperContext = typeof(AlarmApplicationModule);
        }
    }
}
