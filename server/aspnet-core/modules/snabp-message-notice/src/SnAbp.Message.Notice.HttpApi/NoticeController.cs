
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Message.Notice.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Message.Notice
{

    public abstract class NoticeBaseController : AbpController
    {
        protected NoticeBaseController()
        {
            LocalizationResource = typeof(NoticeResource);
        }
    }

    [Route("api/message/notice")]
    public class NoticeController : NoticeBaseController
    {
        public NoticeController()
        {

        }

        [HttpGet,Route("getlist")]
        public Task GetList()
        {
            return null;
        }

    }
}