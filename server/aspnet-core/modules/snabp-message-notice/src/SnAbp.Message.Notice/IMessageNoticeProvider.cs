using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using SnAbp.Message.Service;
using Volo.Abp.Domain.Services;

namespace SnAbp.Message.Notice
{
    public interface IMessageNoticeProvider: IMessageProvider
    {
        Task FileNotice(object data);
    }
}
