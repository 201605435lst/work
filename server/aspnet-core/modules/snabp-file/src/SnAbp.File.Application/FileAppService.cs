
using Volo.Abp.Application.Services;

namespace SnAbp.File
{
    public abstract class FileAppService : ApplicationService
    {
        protected FileAppService()
        {
            LocalizationResource = typeof(FileResource);
            ObjectMapperContext = typeof(FileApplicationModule);
        }
    }
}