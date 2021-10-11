/**********************************************************************
*******命名空间： SnAbp.Quality
*******类 名 称： QualityProblemState
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 18:55:51
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Quality
{
    /// <summary>
    /// $$
    /// </summary>
    public enum QualityFilterType
    {
        [Description("全部")]
        All = 1,
        [Description("我检查的")]
        MyChecked = 2,
        [Description("待我整改")]
        MyWaitingImprove = 3,
        [Description("待我验证")]
        MyWaitingVerify = 4,
        [Description("抄送我的")]
        CopyMine = 5
    }
}
