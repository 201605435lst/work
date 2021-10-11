using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.CrPlan.Enums
{
    public enum RepairLevel
    {
        [Description("天窗点内I级维修")]
        LeveIRepairInSkylight = 1,
        [Description("天窗点内II级维修")]
        LeveIIRepairInSkylight = 2,
        [Description("天窗点外I级维修")]
        LeveIRepairOutSkylight = 3,
        [Description("天窗点外II级维修")]
        LeveIIRepairOutSkylight = 4
    }

    /// <summary>
    /// 计划类型
    /// </summary>
    public enum PlanType
    {
        [Description("垂直天窗")]
        VerticalSkylight = 1,
        [Description("综合天窗")]
        ComprehensiveSkylight = 2,
        [Description("天窗点外")]
        SkylightOutside = 3,
        [Description("全部")]
        All = 4,
        [Description("其他计划")]
        Other = 5
    }
    /// <summary>
    /// 计划内容类型
    /// </summary>
    public enum WorkContentType
    {

        [Description("年月计划")]
        MonthYearPlan = 1,
        [Description("其他计划")]
        OtherPlan = 2,
    }

    public enum PlanState
    {
        [Description("待派工")]
        UnDispatching = 1,
        [Description("已派工")]
        Dispatching = 2,
        [Description("未下发")]
        NotIssued = 3,
        [Description("已下发")]
        Issued = 4,
        [Description("已完成")]
        Complete = 5,
        [Description("审批中")]
        Waitting = 6,
        [Description("待提交")]
        UnSubmited = 7,
        [Description("已撤销")]
        Revoke = 8,
        [Description("已批复")]
        Adopted = 9,
        [Description("未批复")]
        UnAdopted = 10,
        [Description("已退回")]
        Backed = 11,
        [Description("命令取消")]
        OrderCancel = 12,
        [Description("自然灾害取消")]
        NaturalDisasterCancel = 13,
        [Description("其他原因取消")]
        OtherReasonCancel =14
    }

    public enum WorkTicketRltCooperationUnitState
    {
        [Description("已完成")]
        Finish = 1,
        [Description("未完成")]
        UnFinish = 2,
    }
}
