namespace SnAbp.Bpm
{
    /// <summary>
    /// 工作流状态
    /// </summary>
    public enum WorkflowState
    {
        All         = 0,    // 全部
        Waiting     = 1,    // 待审批
        Finished    = 2,    // 已完成
        Stopped     = 3,    // 已终止/拒绝
        Rejected    = 4,    // 被退回
    }
}