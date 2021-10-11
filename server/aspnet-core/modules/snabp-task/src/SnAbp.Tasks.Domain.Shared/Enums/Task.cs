using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Tasks.enums
{
    /// <summary>
    /// 状态类型
    /// </summary>
    public enum StateType
    {
        [Description("进行中")]
        Processing = 1,
        [Description("已结项")]
        Receive = 2,
        [Description("已完成")]
        Finshed = 3,
        [Description("已拒绝")]
        Refused = 4,
        [Description("未启动")]
        NoStart = 5,
        [Description("已暂停")]
        Stop = 6,
    }

    /// <summary>
    /// 优先级
    /// </summary>
    public enum PriorityType
    {
        [Description("紧急重要")]
        ImportantUrgent = 1,
        [Description("重要不紧急")]
        ImportantNoUrgent = 2,
        [Description("紧急不重要")]
        NoImportantUrgent = 3,
        [Description("不紧急不重要")]
        NoImportantNoUrgent = 4,
    }

    /// <summary>
    /// 责任类型
    /// </summary>
    public enum ResponsibleType
    {
        [Description("我发起的")]
        Initial = 1,
        [Description("我负责的")]
        Manager = 2,
        [Description("抄送给我")]
        Cc = 3,
    }
    /// <summary>
    /// 任务群组
    /// </summary>
    public enum TaskGroup
    {
        All = 0,    // 所有的
        Initial = 1,    // 【用户】发起的
        Manage = 2,    // 【用户】负责的
        Cc = 3    // 【用户】参与的
    }

    public enum FileType {
        [Description("我创建的")]
        Created = 1,
        [Description("我反馈的")]
        Feedback = 2,
    }
}
