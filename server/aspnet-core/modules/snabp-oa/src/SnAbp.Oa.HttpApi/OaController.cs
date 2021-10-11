using SnAbp.Oa.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Oa
{
    public abstract class OaController : AbpController
    {
        protected OaController()
        {
            LocalizationResource = typeof(OaResource);
        }
    }
}
