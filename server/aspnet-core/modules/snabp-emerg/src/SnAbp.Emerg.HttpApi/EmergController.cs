using SnAbp.Emerg.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Emerg
{
    public abstract class EmergController : AbpController
    {
        protected EmergController()
        {
            LocalizationResource = typeof(EmergResource);
        }
    }
}
