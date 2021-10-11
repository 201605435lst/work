using SnAbp.Bpm.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Bpm
{
    public abstract class BpmController : AbpController
    {
        protected BpmController()
        {
            LocalizationResource = typeof(BpmResource);
        }
    }
}
