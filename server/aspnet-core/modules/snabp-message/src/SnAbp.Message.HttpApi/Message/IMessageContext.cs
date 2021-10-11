/**********************************************************************
*******命名空间： SnAbp.Message.Message
*******接口名称： IMessageContext
*******接口说明： 消息上下文接口，用来定义顶层消息的常用发送方法，是对HubContext 方法的封装使用
*******作    者： 东腾 Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/1 16:44:50
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2019-2020. All rights reserved ★ *********
***********************************************************************
 */

using System.Threading.Tasks;

using SnAbp.Message.MessageDefine;

namespace SnAbp.Message.Message
{
    public interface IMessageContext<T>
    {
        Task SendAsync(BaseMessage message, string method, string type);
        Task SendAsync(BaseMessage message, string method, string type, object arg1);
        Task SendAsync(BaseMessage message, string method, string type, object arg1, object arg2);
        Task SendAsync(BaseMessage message, string method, string type, object arg1, object arg2, object arg3);
        Task SendAsync(BaseMessage message, string methodName, string type, object arg1, object arg2, object arg3, object arg4);
    }
}