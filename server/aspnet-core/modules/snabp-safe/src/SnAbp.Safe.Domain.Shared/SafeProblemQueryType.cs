/**********************************************************************
*******命名空间： SnAbp.Safe
*******类 名 称： SafeProblemQueryType
*******类 说 明： 数据查询过滤类型
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/11 16:29:06
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Safe
{
    /// <summary>
    /// $$
    /// </summary>
    public enum SafeProblemQueryType
    {
        /// <summary>
        /// 全部
        /// </summary>
        All,
        /// <summary>
        /// 我检查的
        /// </summary>
        Checked,
        /// <summary>
        /// 待整改
        /// </summary>
        WaitInprove,
        /// <summary>
        /// 待验证
        /// </summary>
        WaitVeri,
        /// <summary>
        /// 抄送我的
        /// </summary>
        Cc
    }
}
