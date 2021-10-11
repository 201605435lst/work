using SnAbp.Safe.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Safe
{
    public abstract class SafeController : AbpController
    {
        protected SafeController()
        {
            LocalizationResource = typeof(SafeResource);
        }
    }
}
