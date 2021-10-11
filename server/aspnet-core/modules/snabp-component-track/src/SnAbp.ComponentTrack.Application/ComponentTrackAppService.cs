using SnAbp.ComponentTrack.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.ComponentTrack
{
    public abstract class ComponentTrackAppService : ApplicationService
    {
        protected ComponentTrackAppService()
        {
            LocalizationResource = typeof(ComponentTrackResource);
            ObjectMapperContext = typeof(ComponentTrackApplicationModule);
        }
    }
}
