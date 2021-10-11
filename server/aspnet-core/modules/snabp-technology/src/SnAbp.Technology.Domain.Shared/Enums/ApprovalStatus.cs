/**********************************************************************
*******命名空间： SnAbp.Technology.enums
*******类 名 称： ApprovalStatus
*******类 说 明： 审批状态枚举
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 11:37:57 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Technology.enums
{
    /// <summary>
    /// 审批状态枚举
    /// </summary>
    public enum ApprovalStatus
    {
        All=0, // 所有
        [Description("待提交")]
        ToSubmit = 1,
        [Description("审核中")]
        OnReview = 2,
        [Description("审核通过")]
        Pass = 3,
        [Description("审核未通过")]
        UnPass = 4,
    }
}
