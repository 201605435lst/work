
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Message.Email
{
    public abstract class EmailController : AbpController
    {
        protected EmailController()
        {
           // LocalizationResource = typeof(EmailResource);
        }
    }
}
