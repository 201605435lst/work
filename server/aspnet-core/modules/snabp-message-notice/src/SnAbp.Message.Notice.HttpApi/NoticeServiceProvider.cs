using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SnAbp.Message.Message;
using SnAbp.Message.MessageDefine;
using SnAbp.Message.Notice.IRepositorys;
using Volo.Abp.Guids;

namespace SnAbp.Message.Notice
{
    /// <summary>
    /// 消息服务处理类，在这里实现模块间消息接收处理逻辑
    /// </summary>
    [MessageServiceProvider(MessageBaseDefine.MessageType)]
    public class NoticeServiceProvider : MessageServiceProvider
    {
        IMessageContext<NoticeHub> MessageContext { get; }
        IGuidGenerator GuidGenerator { get; }
        INoticeMessageRepository NoticeMessageRepository { get; }
        public NoticeServiceProvider(
            IMessageContext<NoticeHub> hub,
            IGuidGenerator guidGenerator,
            INoticeMessageRepository noticeMessageRepository
            )
        {
            MessageContext = hub;
            GuidGenerator = guidGenerator;
            NoticeMessageRepository = noticeMessageRepository;
        }
        /// <summary>
        /// 接收各功能模块发送过来的数据
        /// </summary>
        /// <param name="data">二进制的数据信息</param>
        /// <returns></returns>
        public override async Task Receive(byte[] data)
        {
            var message = data.GetMessage<NoticeMessage>();

            if (message != null)
            {
                // 添加消息数据
                var content =(NoticeMessageContent)message.Content;
                if (message.GetUserIds().Any())
                {
                    foreach (var id in message.GetUserIds())
                    {
                        var notice = new Entities.Notice(GuidGenerator.Create())
                        {
                            UserId = id,
                            Content = content.Content,
                            Type = content.Type,
                            Process=false,
                            CreatorId =content.SponsorId
                        };
                        var result =await NoticeMessageRepository.InsertAsync(notice);
                        message.SendData = result;
                        await MessageContext.SendAsync(message, "ReceiveMessage", MessageBaseDefine.MessageType);
                    }
                }
            }
            // 解析消息，根据类型发送
            await MessageContext.SendAsync(message, "ReceiveMessage", MessageBaseDefine.MessageType);
        }
    }
}
