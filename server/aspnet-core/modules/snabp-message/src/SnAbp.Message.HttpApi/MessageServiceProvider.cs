/**********************************************************************
*******命名空间： SnAbp.Message.MessageController
*******类 名 称： MessageController
*******类 说 明： 消息服务的提供者，是一个用于规范的自模块消息实现的抽象类型，具体的实现在各个自模块中
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/16 14:51:18
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
*/

using System.Threading.Tasks;

namespace SnAbp.Message
{
    public abstract class MessageServiceProvider : IMessageServiceProvider
    {
        public async Task SendMessage(byte[] data) => await Receive(data);
        public abstract Task Receive(byte[] data);
    }
}