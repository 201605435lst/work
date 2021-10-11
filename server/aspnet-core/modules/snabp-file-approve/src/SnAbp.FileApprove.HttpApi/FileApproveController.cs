using SnAbp.FileApprove.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.FileApprove
{
    public abstract class FileApproveController : AbpController
    {
        protected FileApproveController()
        {
            LocalizationResource = typeof(FileApproveResource);
        }
    }
}
