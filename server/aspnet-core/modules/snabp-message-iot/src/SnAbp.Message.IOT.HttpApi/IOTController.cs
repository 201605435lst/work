using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Message.IOT
{
    public abstract class IOTController : AbpController
    {
        protected IOTController()
        {
           // LocalizationResource = typeof(IOTResource);
        }
    }
}
