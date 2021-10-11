using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.CrPlan.Enums
{
    /// <summary>
    /// 职责
    /// </summary>
    public enum Duty
    {
        [Description("检修")]
        Recondition = 0,
        [Description("验收")]
        Acceptance = 1
    }

    /// <summary>
    /// 人员职责
    /// </summary>
    public enum UserDuty
    {
        [Description("作业成员")]
        WorkMembers = 0,
        [Description("作业组长")]
        WorkLeader = 1,
        [Description("现场防护员")]
        FieldGuard = 2,
        [Description("驻站联络员")]
        StationLiaisonOfficer = 3
    }

    /// <summary>
    /// 派工单状态
    /// </summary>
    public enum OrderState
    {
        [Description("未完成")]
        Unfinished = 0,
        [Description("已完成")]
        Complete = 1,
        [Description("已验收")]
        Acceptance = 2,
        [Description("命令取消")]
        OrderCancel = 3,
        [Description("自然灾害取消")]
        NaturalDisasterCancel = 4,
        [Description("其他原因取消")]
        OtherReasonCancel = 5

    }

    /// <summary>
    /// 派工单类型
    /// </summary>
    public enum OrderType
    {
        [Description("派工作业")]
        WorkOrder = 0,
        [Description("其他作业")]
        OtherAssignments = 1
    }

    /// <summary>
    /// 检修设备验收状态
    /// </summary>
    public enum AcceptanceResults
    {
        [Description("未完成")]
        Unfinished = 0,
        [Description("已完成")]
        Complete = 1
    }
}
