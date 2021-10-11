using SnAbp.ComponentTrack.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.ComponentTrack
{
    public abstract class ComponentTrackController : AbpController
    {
        protected ComponentTrackController()
        {
            LocalizationResource = typeof(ComponentTrackResource);
        }
    }
}
