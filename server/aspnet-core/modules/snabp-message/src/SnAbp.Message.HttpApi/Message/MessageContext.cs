/**********************************************************************
*******命名空间： SnAbp.Message.Message
*******类 名 称： MessageContext
*******类 说 明： 消息上下文的顶级实现，可提供方法，让自模块调取发送使用
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/1 16:48:11
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using SnAbp.Message.Hubs;
using SnAbp.Message.MessageDefine;
using SnAbp.Message.Services;

using Volo.Abp.AspNetCore.SignalR;

namespace SnAbp.Message.Message
{
    public class MessageContext<T> : IMessageContext<T> where T : AbpHub
    {
        public MessageContext(
            IHubContext<T> hubContext,
            MessageManager messageManager
        )
        {
            HubContext = hubContext;
            MessageManager = messageManager;
        }

        IHubContext<T> HubContext { get; }
        MessageManager MessageManager { get; }

        public async Task SendAsync(BaseMessage message, string method, string type) =>
            await SendAsync(message, method, type, null);

        public async Task SendAsync(BaseMessage message, string method, string type, object arg1) =>
            await SendAsync(message, method, type, arg1, null);

        public async Task SendAsync(BaseMessage message, string method, string type, object arg1, object arg2) =>
            await SendAsync(message, method, type, arg1, arg2, null);

        public async Task SendAsync(BaseMessage message, string method, string type, object arg1, object arg2, object arg3) =>
            await SendAsync(message, method, type, arg1, arg2, arg3, null);

        public async Task SendAsync(BaseMessage message, string method, string type, object arg1, object arg2, object arg3,
            object arg4) =>
            await SendMessage(message, method, type, arg1, arg2, arg3, arg4);

        /// <summary>
        ///     根据类型发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="method">客户端方法</param>
        /// <param name="type">接收类型</param>
        async Task SendMessage(BaseMessage message, string method, string type, object arg1 = null, object arg2 = null,
            object arg3 = null, object arg4 = null)
        {
            if (message == null) return;
            IReadOnlyList<string> ids = null;
            switch (message.SendType)
            {
                case SendModeType.User:
                    ids = message.GetTargetIds().ToStringList();
                    await HubContext.SendByUserIds(method, ids, type, message.SendData, arg1, arg2, arg3, arg4);
                    break;
                case SendModeType.Organization:
                    ids = (await MessageManager.GetUserIdsByOrganization(message.GetTargetIds())).ToStringList();
                    await HubContext.SendByUserIds(method, ids, type, message.SendData, arg1, arg2, arg3, arg4);
                    break;

                case SendModeType.Role:
                    ids = (await MessageManager.GetUserIdsByRole(message.GetTargetIds())).ToStringList();
                    await HubContext.SendByUserIds(method, ids, type, message.SendData, arg1, arg2, arg3, arg4);
                    break;
                case SendModeType.Member:
                    ids = (await MessageManager.GetUserIdsByMember(
                            userIds: message.GetUserIds(),
                            organizationIds: message.GetOrganizationIds(),
                            roleIds: message.GetRoleIds())
                        ).ToStringList();
                    await HubContext.SendByUserIds(method, ids, type, message.SendData, arg1, arg2, arg3, arg4);
                    break;
                case SendModeType.Default:
                default:
                    await HubContext.SendAllAsync(method, type, message.SendData, arg1, arg2, arg3, arg4);
                    break;
            }
        }
    }
}