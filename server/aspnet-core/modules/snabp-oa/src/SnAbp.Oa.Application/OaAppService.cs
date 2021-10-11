using SnAbp.Oa.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Oa
{
    public abstract class OaAppService : ApplicationService
    {
        protected OaAppService()
        {
            LocalizationResource = typeof(OaResource);
            ObjectMapperContext = typeof(OaApplicationModule);
        }
    }
}
