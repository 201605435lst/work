using System.ComponentModel;

namespace SnAbp.Material.Enums
{
    /// <summary>
    /// 领料单状态
    /// </summary>
    public enum MaterialOfBillState
    {
        [Description("待提交")]
        UnSubmitted = 1,
        [Description("待审核")]
        Waitting = 2,
        [Description("已通过")]
        Passed = 3,
    }
}
