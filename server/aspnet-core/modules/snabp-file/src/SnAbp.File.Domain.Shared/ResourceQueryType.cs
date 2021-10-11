/**********************************************************************
*******命名空间： SnAbp.File
*******类 名 称： ResourceQueryType
*******类 说 明： 资源查询枚举类
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 10:37:43
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.File
{
    public enum ResourceQueryType
    {
        /// <summary>
        /// 没有被删除
        /// </summary>
        NotDelete,
        /// <summary>
        /// 已经删除的
        /// </summary>
        Deleted,
        /// <summary>
        /// 没有共享的
        /// </summary>
        NotShared,
        /// <summary>
        /// 已共享
        /// </summary>
        Shared,
        /// <summary>
        /// 没有公开，属于个人资源
        /// </summary>
        UnPublish,
        /// <summary>
        /// 已公开，不属于个人资源
        /// </summary>
        Publish
    }
}
