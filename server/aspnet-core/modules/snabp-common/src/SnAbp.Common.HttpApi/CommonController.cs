using SnAbp.Common.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Common
{
    public abstract class CommonController : AbpController
    {
        protected CommonController()
        {
            LocalizationResource = typeof(CommonResource);
        }
    }
}
