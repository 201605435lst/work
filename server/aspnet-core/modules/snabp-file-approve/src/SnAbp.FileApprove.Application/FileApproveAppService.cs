using SnAbp.FileApprove.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.FileApprove
{
    public abstract class FileApproveAppService : ApplicationService
    {
        protected FileApproveAppService()
        {
            LocalizationResource = typeof(FileApproveResource);
            ObjectMapperContext = typeof(FileApproveApplicationModule);
        }
    }
}
