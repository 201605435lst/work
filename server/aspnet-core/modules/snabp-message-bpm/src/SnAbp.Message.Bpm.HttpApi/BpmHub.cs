using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

using SnAbp.Identity;
using SnAbp.Message.Bpm.Dtos;
using SnAbp.Message.Bpm.Entities;
using SnAbp.Message.Bpm.Services;
using SnAbp.Message.Hubs;
using SnAbp.Message.Services;

using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Users;

namespace SnAbp.Message.Bpm
{
    //[Authorize]
    public class BpmHub : AbpHub, IBaseHub
    {
        IBpmMessageRepository MessageRepository { get; }
        public BpmHub(IBpmMessageRepository messageRepository) => MessageRepository = messageRepository;
        /// <summary>
        /// 客户端注册及获取未读的消息
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task Register(string topic)
        {
            var userId = Context.UserIdentifier;
            if (!userId.IsNullOrEmpty())
            {
                var data = await MessageRepository.GetNoProcessMessage(userId);
                if (data.Any())
                {
                    var list = new List<BpmMessageDto>();
                    data.ForEach(a => list.Add(MessageDataHandler.TransData(a)));
                    list = list.OrderByDescending(a => a.CreateTime).ToList();
                    await Clients.User(userId).SendAsync("register", list.Serialize());
                }
            }
        }
        [HubMethodName("Process")]
        public async Task ProcessMessage(string messageId)
        {
            await MessageRepository.Update(messageId);
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            return null;
        }
       
    }
}
