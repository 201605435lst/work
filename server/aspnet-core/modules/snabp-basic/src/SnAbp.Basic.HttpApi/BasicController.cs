using SnAbp.Basic.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Basic
{
    public abstract class BasicController : AbpController
    {
        protected BasicController()
        {
            LocalizationResource = typeof(BasicResource);
        }
    }
}
