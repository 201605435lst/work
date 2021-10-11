using SnAbp.Material.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Material
{
    public abstract class MaterialAppService : ApplicationService
    {
        protected MaterialAppService()
        {
            LocalizationResource = typeof(MaterialResource);
            ObjectMapperContext = typeof(MaterialApplicationModule);
        }
    }
}
