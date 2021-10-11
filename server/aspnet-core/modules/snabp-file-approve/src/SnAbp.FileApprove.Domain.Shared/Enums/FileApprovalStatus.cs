using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.FileApprove.Enums
*文件名：ApprovalStatus
*创建人： liushengtao
*创建时间：2021/9/1 10:25:32
*描述：
*
***********************************************************************/
namespace SnAbp.FileApprove.Enums
{
    /// <summary>
    /// 审批状态枚举
    /// </summary>
    public enum FileApprovalStatus
    {
        All = 0, // 所有
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
