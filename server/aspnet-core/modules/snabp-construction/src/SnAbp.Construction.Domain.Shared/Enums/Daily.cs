using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Construction.Enums
*文件名：Daily
*创建人： liushengtao
*创建时间：2021/7/21 11:03:12
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Enums
{
	/// <summary>
	/// 总体计划审批状态
	/// </summary>
	public enum UnplannedTaskType
	{
		[Description("临时任务")]
		TemporaryDuty = 1,
		[Description("其他任务")]
		OtherDuty = 2,
	}
    public enum DailyStatus
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
