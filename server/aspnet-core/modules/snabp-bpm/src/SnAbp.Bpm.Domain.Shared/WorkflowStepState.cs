namespace SnAbp.Bpm
{
    public enum WorkflowStepState
    {
        Approved    = 1,    // 通过
        Rejected    = 2,    // 退回
        Stopped     = 3,     // 拒绝
    }
}