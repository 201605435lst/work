using SnAbp.StdBasic.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.StdBasic
{
    public abstract class StdBasicController : AbpController
    {
        protected StdBasicController()
        {
            LocalizationResource = typeof(StdBasicResource);
        }
    }
}
