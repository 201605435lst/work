using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Schedule.Enums
{
    /// <summary>
    /// 工作类型
    /// </summary>
    public enum WorkType
    {
        [Description("自动计算")]
        AutoCompute = 1,
        [Description("里程碑")]
        Milestone = 2,
        [Description("关键工作")]
        Important = 3,
        [Description("非关键工作")]
        Unimportant = 4,
    }

    /// <summary>
    /// 状态类型
    /// </summary>
    public enum State
    {
        [Description("进行中")]
        Processing = 1,
        [Description("已完成")]
        Finshed = 2,
        [Description("已拒绝")]
        Refused = 3,
        [Description("未启动")]
        NoStart = 4,
        [Description("已暂停")]
        Stop = 5,
    }

    /// <summary>
    /// 审批-人员类型
    /// </summary>
    public enum PersonType
    {
        [Description("我发起的")]
        Initial = 1,
        [Description("负责人")]
        Manager = 2,
        [Description("施工员")]
        Worker = 3,
    }
}
