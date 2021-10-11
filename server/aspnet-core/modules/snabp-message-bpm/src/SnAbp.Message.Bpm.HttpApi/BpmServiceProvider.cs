using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SnAbp.Bpm;
using SnAbp.Message.Bpm.Entities;
using SnAbp.Message.Bpm.Services;
using SnAbp.Message.Message;
using SnAbp.Message.MessageDefine;

using Volo.Abp.Guids;

namespace SnAbp.Message.Bpm
{
    /// <summary>
    /// 消息服务处理类，在这里实现模块间消息接收处理逻辑
    /// </summary>
    [MessageServiceProvider(MessageBaseDefine.MessageType)]
    public class BpmServiceProvider : MessageServiceProvider
    {
        IMessageContext<BpmHub> MessageContext { get; }
        IGuidGenerator GuidGenerator { get; }
        IBpmMessageRepository MessageRepository { get; }
        public BpmServiceProvider(
            IMessageContext<BpmHub> hub,
            IGuidGenerator guidGenerator,
            IBpmMessageRepository messageRepository
            )
        {
            MessageContext = hub;
            GuidGenerator = guidGenerator;
            MessageRepository = messageRepository;
        }
        /// <summary>
        /// 接收各功能模块发送过来的数据
        /// </summary>
        /// <param name="data">二进制的数据信息</param>
        /// <returns></returns>
        public override async Task Receive(byte[] data)
        {
            var message = data.GetMessage<BpmMessage>();
            // 解析消息，根据类型发送
            if (message != null)
            {
                var content = (BpmMessageContent)message.Content;
                if (message.GetUserIds().Any())
                {
                    foreach (var id in message.GetUserIds())
                    {
                        var messageModel = new BpmRltMessage(GuidGenerator.Create())
                        {
                            UserId = id,
                            ProcessorId = content.ProcessorId,
                            State = (WorkflowState)content.State,
                            Process = false,
                            Type = content.Type,
                            WorkflowId = content.WorkFlowId,
                            SponsorId = content.SponsorId
                        };
                        // 保存消息
                        var result = await MessageRepository.Insert(messageModel);

                        message.SendData = MessageDataHandler.TransData(result);
                        await MessageContext.SendAsync(message, "ReceiveMessage", MessageBaseDefine.MessageType);

                    }
                }

            }
        }
    }
}
