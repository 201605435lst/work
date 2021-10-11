
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.File
{
    public abstract class FileController : AbpController
    {
        protected FileController()
        {
            LocalizationResource = typeof(FileResource);
        }
    }
}