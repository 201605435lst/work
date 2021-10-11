namespace SnAbp.Bpm
{
    /// <summary>
    /// 用户工作流群组
    /// </summary>
    public enum UserWorkflowGroup
    {
        All         = 0,    // 所有的      
        Initial     = 1,    // 【用户】发起的
        Waiting  = 2,    // 等等【用户】审批的
        Approved    = 3,    // 【用户】审批过的
        Cc          = 4     // 抄送给【用户】的
    }
}