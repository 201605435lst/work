/**********************************************************************
*******命名空间： SnAbp.Message
*******类 名 称： HubContextExtension
*******类 说 明： 集线器扩展方法
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/16 10:53:27
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using SnAbp.Message.MessageDefine;

namespace SnAbp.Message
{
    public static class HubContextExtension
    {
        #region 1、发送所有客户端扩展封装

        /// <summary>
        ///     给所有连接的客户端发送
        /// </summary>
        /// <typeparam name="THub"></typeparam>
        /// <param name="hubContext"></param>
        /// <param name="method"></param>
        /// <param name="arg1"></param>
        public static Task SendAllAsync<THub>(
            this IHubContext<THub> hubContext,
            string method,
            object arg1)
            where THub : Hub => hubContext.Clients.All.SendAsync(method, arg1.Serialize());

        public static Task SendAllAsync<THub>(
            this IHubContext<THub> hubContext,
            string method,
            object arg1,
            object arg2
        )
            where THub : Hub => hubContext.Clients.All.SendAsync(method, arg1.Serialize(), arg2.Serialize());

        public static Task SendAllAsync<THub>(
            this IHubContext<THub> hubContext,
            string method,
            object arg1,
            object arg2,
            object arg3
        )
            where THub : Hub => hubContext.Clients.All.SendAsync(method, arg1.Serialize(), arg2.Serialize(), arg3.Serialize());

        public static Task SendAllAsync<THub>(
            this IHubContext<THub> hubContext,
            string method,
            object arg1,
            object arg2,
            object arg3,
            object arg4
        )
            where THub : Hub => hubContext.Clients.All.SendAsync(method, arg1.Serialize(), arg2.Serialize(), arg3.Serialize(), arg4.Serialize());
        public static Task SendAllAsync<THub>(
            this IHubContext<THub> hubContext,
            string method,
            object arg1,
            object arg2,
            object arg3,
            object arg4,
            object arg5
        )
            where THub : Hub => hubContext.Clients.All.SendAsync(method, 
            arg1.Serialize(),
            arg2.Serialize(), 
            arg3.Serialize(), 
            arg4.Serialize(),
            arg5.Serialize()
            );
        public static Task SendAllAsync<THub>(
            this IHubContext<THub> hubContext,
            string method,
            object arg1,
            object arg2,
            object arg3,
            object arg4,
            object arg5,
            object arg6
        )
            where THub : Hub => hubContext.Clients.All.SendAsync(method,
            arg1.Serialize(),
            arg2.Serialize(),
            arg3.Serialize(),
            arg4.Serialize(),
            arg5.Serialize(),
            arg6.Serialize()
        );
        #endregion

        #region 2、发送给指定用户

        public static Task SendByUserIds<THub>(
            this IHubContext<THub> hubContext,
            string method,
            IReadOnlyList<string> ids,
            object arg1,
            object arg2
        ) where THub : Hub => hubContext.Clients.Users(ids).SendAsync(method, arg1.Serialize(), arg2.Serialize());

        public static Task SendByUserIds<THub>(
            this IHubContext<THub> hubContext,
            string method,
            IReadOnlyList<string> ids,
            object arg1
        ) where THub : Hub => hubContext.Clients.Users(ids)
            .SendAsync(method, arg1.Serialize());

        public static Task SendByUserIds<THub>(
            this IHubContext<THub> hubContext,
            string method,
            IReadOnlyList<string> ids,
            object arg1,
            object arg2,
            object arg3
        ) where THub : Hub => hubContext.Clients.Users(ids).SendAsync(method, arg1.Serialize(), arg2.Serialize(), arg3.Serialize());
        public static Task SendByUserIds<THub>(
            this IHubContext<THub> hubContext,
            string method,
            IReadOnlyList<string> ids,
            object arg1,
            object arg2,
            object arg3,
            object arg4
        ) where THub : Hub => hubContext.Clients.Users(ids)
            .SendAsync(method, 
                arg1.Serialize(), 
                arg2.Serialize(), 
                arg3.Serialize(),
                arg4.Serialize()
         );
        public static Task SendByUserIds<THub>(
            this IHubContext<THub> hubContext,
            string method,
            IReadOnlyList<string> ids,
            object arg1,
            object arg2,
            object arg3,
            object arg4,
            object arg5
        ) where THub : Hub => hubContext.Clients.Users(ids)
            .SendAsync(method,
                arg1.Serialize(),
                arg2.Serialize(),
                arg3.Serialize(),
                arg4.Serialize(),
                arg5.Serialize()
            );
        public static Task SendByUserIds<THub>(
            this IHubContext<THub> hubContext,
            string method,
            IReadOnlyList<string> ids,
            object arg1,
            object arg2,
            object arg3,
            object arg4,
            object arg5,
            object arg6
        ) where THub : Hub => hubContext.Clients.Users(ids)
            .SendAsync(method,
                arg1.Serialize(),
                arg2.Serialize(),
                arg3.Serialize(),
                arg4.Serialize(),
                arg5.Serialize(),
                arg6.Serialize()
            );
        #endregion
    }
}