using System.ComponentModel;

namespace SnAbp.Construction.Enums
{
    /// <summary>
    /// 安全防护措施
    /// </summary>
    public enum SafetyMeasure
    {
        [Description("内部培训")]
        InternalTraining = 1,
        [Description("自身装备")]
        Ownar = 2,
        [Description("现场环境无安全隐患")]
        NoSafetyRisk = 3,
    }

    /// <summary>
    /// 工序控制类型
    /// </summary>
    public enum ControlType
    {
        [Description("关键工序")]
        KeyProcess = 1,
        [Description("一般工序")]
        GeneralProcess = 2,
        [Description("隐蔽")]
        HideProcess = 3,
        [Description("旁站")]
        SideProcess = 4,
    }

    /// <summary>
    /// 派工单审批状态
    /// </summary>
    public enum DispatchState
    {
        All = 0, // 所有
        [Description("待提交")]
        UnSubmit = 1,
        [Description("审核中")]
        OnReview = 2,
        [Description("已通过")]
        Pass = 3,
        [Description("已驳回")]
        UnPass = 4,
    }
}