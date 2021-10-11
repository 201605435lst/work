/**********************************************************************
*******命名空间： SnAbp.Message.Hubs
*******类 名 称： BaseHub
*******类 说 明： 基础集线器 (the base hub class deals with some of the logic that services interact with client data notification)
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/12 16:40:58
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.SignalR;

namespace SnAbp.Message.Hubs
{
    
    public interface IBaseHub
    {
        Task Register(string topic);
    }
}