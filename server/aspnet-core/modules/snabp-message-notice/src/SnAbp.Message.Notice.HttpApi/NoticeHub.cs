using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SnAbp.Message.Hubs;
using SnAbp.Message.Notice.IRepositorys;
using Volo.Abp.AspNetCore.SignalR;

namespace SnAbp.Message.Notice
{
    public class NoticeHub : AbpHub, IBaseHub
    {
        INoticeMessageRepository NoticeMessageRepository { get; }

        public NoticeHub(INoticeMessageRepository noticeMessageRepository) => NoticeMessageRepository = noticeMessageRepository;
        // 注册实现
        public async Task Register(string topic)
        {
            var userid = Context.UserIdentifier;
            var data = await NoticeMessageRepository.GetNoProcessMessage(userid);
            if (data != null)
            {
                data = data.Where(x=>!x.Process).OrderByDescending(a => a.CreationTime).ToList();
                await Clients.User(userid).SendAsync("register", data.Serialize());
            }
        }
    }
}
